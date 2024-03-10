using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayFromFirstScene
{
    private static string previousScenePath = "";

    static PlayFromFirstScene()
    {
        EditorApplication.playModeStateChanged += ChangeSceneOnPlayMode;
    }

    static void ChangeSceneOnPlayMode(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // Save the currently open scene path
            previousScenePath = SceneManager.GetActiveScene().path;

            // Your start scene path
            string startScenePath = "Assets/Scenes/Main Menu.unity";
            if (SceneManager.GetActiveScene().path != startScenePath)
            {
                // Optionally save changes to the current scene
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                // Open the start scene
                EditorSceneManager.OpenScene(startScenePath);
            }
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            // When exiting play mode, return to the previously active scene
            if (!string.IsNullOrEmpty(previousScenePath))
            {
                EditorSceneManager.OpenScene(previousScenePath);
            }
        }
    }
}