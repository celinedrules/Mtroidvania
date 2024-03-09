using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages saving and loading game states as JSON files. It supports operations like saving the current state,
/// loading a saved state, listing available saves, and deleting a save file.
/// </summary>
public class JsonSavingSystem : Singleton<JsonSavingSystem>
{
    // File extension for save files.
    private const string Extension = ".json";
    
    /// <summary>
    /// Asynchronously loads the last scene indicated in the saved game state and restores the state.
    /// </summary>
    /// <param name="saveFile">The file name of the save to load.</param>
    /// <returns>An IEnumerator for coroutine support.</returns>
    public IEnumerator LoadLastScene(string saveFile)
    {
        JObject state = LoadJsonFromFile(saveFile);
        IDictionary<string, JToken> stateDictionary = state;
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (stateDictionary.ContainsKey("lastSceneBuildIndex"))
            buildIndex = (int)stateDictionary["lastSceneBuildIndex"];

        yield return SceneManager.LoadSceneAsync(buildIndex);

        RestoreFromToken(state);
    }
    
    /// <summary>
    /// Saves the current game state to a JSON file.
    /// </summary>
    /// <param name="saveFile">The file name to save the game state under.</param>
    public void Save(string saveFile)
    {
        JObject state = LoadJsonFromFile(saveFile); // Load existing state to allow incremental saves.
        CaptureAsToken(state); // Capture current state.
        SaveFileAsJson(saveFile, state); // Write state to file.
    }
    
    /// <summary>
    /// Deletes a specific save file.
    /// </summary>
    /// <param name="saveFile">The file name of the save to delete.</param>
    public void Delete(string saveFile) => File.Delete(GetPathFromSaveFile(saveFile));

    /// <summary>
    /// Loads a game state from a JSON file and restores it.
    /// </summary>
    /// <param name="saveFile">The file name of the save to load.</param>
    public void Load(string saveFile) => RestoreFromToken(LoadJsonFromFile(saveFile));

    /// <summary>
    /// Lists all save files in the persistent data path.
    /// </summary>
    /// <returns>An IEnumerable of type string of save file names without the extension.</returns>
    public IEnumerable<string> ListSaves() => from path in Directory.EnumerateFiles(Application.persistentDataPath)
                                               where Path.GetExtension(path) == Extension
                                               select Path.GetFileNameWithoutExtension(path);
    
    // Loads JSON state from a file, or returns an empty JObject if the file does not exist.
    private JObject LoadJsonFromFile(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);

        if (!File.Exists(path))
        {
            Debug.Log(path);
            return new JObject();
        }
        
        using StreamReader textReader = File.OpenText(path);
        using JsonTextReader reader = new(textReader) { FloatParseHandling = FloatParseHandling.Double };

        return JObject.Load(reader);
    }
    
    // Saves the given JObject as a formatted JSON file.
    private void SaveFileAsJson(string saveFile, JObject state)
    {
        string path = GetPathFromSaveFile(saveFile);
        Debug.Log("Saving to " + path); // Use Debug.Log for Unity logging.

        using StreamWriter textWriter = File.CreateText(path);
        using JsonTextWriter writer = new(textWriter) { Formatting = Formatting.Indented };
        state.WriteTo(writer);
    }
    
    // Captures the current state of all JsonSavableEntity objects in the scene, including scene index.
    private void CaptureAsToken(JObject state)
    {
        IDictionary<string, JToken> stateDictionary = state;

        var entities = FindObjectsByType<JsonSavableEntity>(FindObjectsSortMode.None);

        foreach (JsonSavableEntity savable in entities)
            stateDictionary[savable.GetUniqueIdentifier()] = savable.CaptureAsJToken();

        stateDictionary["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Save: " + stateDictionary["lastSceneBuildIndex"]);
    }
    
    // Restores the state of all JsonSavableEntity objects in the scene from the given JObject.
    private void RestoreFromToken(JObject state)
    {
        IDictionary<string, JToken> stateDictionary = state;

        var entities = FindObjectsByType<JsonSavableEntity>(FindObjectsSortMode.None);

        foreach (JsonSavableEntity savable in entities)
        {
            string id = savable.GetUniqueIdentifier();

            if (stateDictionary.ContainsKey(id))
                savable.RestoreFromJToken(stateDictionary[id]);
        }

        GameManager.Instance.GameLoaded = true;
        GameManager.Instance.IsNewGame = false;
        GameManager.Instance.Load(stateDictionary["lastSceneBuildIndex"].ToObject<int>());
    }
    
    // Constructs the full file path for a given save file name.
    private string GetPathFromSaveFile(string saveFile)
    {
        // Check if saveFile already ends with ".json", if not, append ".json" extension
        string fileName = saveFile.EndsWith(Extension) ? saveFile : saveFile + Extension;
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    
    // This method is added to your JsonSavingSystem class.
// It tries to find and return the saved JToken state for a specific JsonSavableEntity.
    public JToken GetSavedStateFor(JsonSavableEntity entity)
    {
        // Attempt to load the current save file's JSON content.
        // You might need to adjust how you get the current save file's name.
        string file = GetPathFromSaveFile("save");
        JObject currentState = LoadJsonFromFile(file);

        // Attempt to find the saved state using the entity's unique identifier.
        // Ensure that the loaded state is not null and contains the key before attempting to access it.
        if (currentState != null && currentState.ContainsKey(entity.GetUniqueIdentifier()))
        {
            return currentState[entity.GetUniqueIdentifier()];
        }
    
        // If not found, return null or a new JObject as a default state.
        return null;
    }

}
