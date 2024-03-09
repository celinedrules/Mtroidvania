using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A MonoBehaviour that enables any GameObject to be saved and restored using JSON.
/// Uses the IJsonSavable interface for components that support JSON serialization.
/// </summary>
[ExecuteAlways]
public class JsonSavableEntity : MonoBehaviour
{
    // A lookup dictionary to ensure uniqueness of identifiers across all entities.
    private static Dictionary<string, JsonSavableEntity> _globalLookup = new();

    // A unique identifier for each entity, set in the Editor.
    [SerializeField] private string uniqueIdentifier = "";

    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    /// <returns>The unique identifier as a string.</returns>
    public string GetUniqueIdentifier() => uniqueIdentifier;

    /// <summary>
    /// Captures the state of all IJsonSavable components on this GameObject as a JSON token.
    /// </summary>
    /// <returns>A JToken representing the serialized state of this entity.</returns>
    public JToken CaptureAsJToken()
    {
        JObject state = new();
        IDictionary<string, JToken> stateDictionary = state;
        // Retrieve all components that can be saved as JSON.
        IJsonSavable[] jsonSavables = GetComponents<IJsonSavable>();

        foreach (IJsonSavable jsonSavable in jsonSavables)
        {
            // Serialize each component to a JToken.
            JToken token = jsonSavable.CaptureAsJToken();
            // Use the component's type name as the key.
            stateDictionary[jsonSavable.GetType().ToString()] = token;
        }

        return state;
    }
    
    /// <summary>
    /// Restores the state of all IJsonSavable components on this GameObject from a JSON token.
    /// </summary>
    /// <param name="s">The JToken representing the serialized state to restore from.</param>
    public void RestoreFromJToken(JToken s)
    {
        JObject state = s.ToObject<JObject>();
        IDictionary<string, JToken> stateDictionary = state;
        // Retrieve all components that can be restored from JSON.
        IJsonSavable[] jsonSavables = GetComponents<IJsonSavable>();

        foreach (IJsonSavable jsonSavable in jsonSavables)
        {
            // Find the serialized state for each component.
            string component = jsonSavable.GetType().ToString();

            if (stateDictionary.ContainsKey(component))
                // Restore the component's state from its serialized form.
                jsonSavable.RestoreFromJToken(stateDictionary[component]);
        }
    }
    
    // Checks if a given identifier is unique within the global lookup.
    private bool IsUnique(string candidate)
    {
        // Several conditions to determine uniqueness.
        if (!_globalLookup.ContainsKey(candidate))
            return true;

        if (_globalLookup[candidate] == this)
            return true;

        if (_globalLookup[candidate] is null)
        {
            _globalLookup.Remove(candidate);
            return true;
        }

        if (_globalLookup[candidate].GetUniqueIdentifier() != candidate)
        {
            _globalLookup.Remove(candidate);
            return true;
        }

        return false;
    }
    
#if UNITY_EDITOR
    // In the Unity Editor, updates the unique identifier and maintains the global lookup.
    private void Update()
    {
        // Avoid running in play mode or without a valid scene path.
        if (Application.IsPlaying(gameObject))
            return;

        if (string.IsNullOrEmpty(gameObject.scene.path))
            return;

        // Access and modify the uniqueIdentifier property.
        SerializedObject serializedObject = new(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
        
        // Generate a new unique identifier if necessary.
        // TODO: Fix when uncommenting the players UniqueId should be player in both scenes
        if (string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        // Update the global lookup.
        _globalLookup[property.stringValue] = this;
    }
#endif
}
