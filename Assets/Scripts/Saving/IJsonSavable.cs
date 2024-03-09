using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Defines an interface for objects that can be serialized to and deserialized from JSON.
/// </summary>
public interface IJsonSavable
{
    /// <summary>
    /// Captures the current state of the object as a JSON token.
    /// </summary>
    /// <returns>A JToken representing the object's current state.</returns>
    JToken CaptureAsJToken();

    /// <summary>
    /// Restores the object's state from a JSON token.
    /// </summary>
    /// <param name="state">The JSON token containing the state to restore.</param>
    void RestoreFromJToken(JToken state);
}

/// <summary>
/// Provides extension methods for JSON serialization and deserialization of Unity's Vector2 type.
/// </summary>
public static class JsonStatics
{
    /// <summary>
    /// Converts a Vector2 instance to a JToken (JSON object) with "x" and "y" properties.
    /// </summary>
    /// <param name="vector">The Vector2 instance to convert.</param>
    /// <returns>A JToken representing the Vector2.</returns>
    public static JToken ToToken(this Vector2 vector)
    {
        // Initialize a new JObject to store the vector's state
        JObject state = new();
        // Using a dictionary to easily map "x" and "y" keys to their respective values
        IDictionary<string, JToken> stateDictionary = state;
        stateDictionary["x"] = vector.x;
        stateDictionary["y"] = vector.y;

        // Return the constructed JObject as a JToken
        return state;
    }

    /// <summary>
    /// Converts a Vector3 instance to a JToken (JSON object) with "x", "y", and "z" properties.
    /// </summary>
    /// <param name="vector">The Vector2 instance to convert.</param>
    /// <returns>A JToken representing the Vector2.</returns>
    public static JToken ToToken(this Vector3 vector)
    {
        // Initialize a new JObject to store the vector's state
        JObject state = new();
        // Using a dictionary to easily map "x", "y", and "z" keys to their respective values
        IDictionary<string, JToken> stateDictionary = state;
        stateDictionary["x"] = vector.x;
        stateDictionary["y"] = vector.y;
        stateDictionary["z"] = vector.z;

        // Return the constructed JObject as a JToken
        return state;
    }
    
    /// <summary>
    /// Converts a JToken (expected to be a JSON object with "x" and "y" properties) to a Vector2 instance.
    /// </summary>
    /// <param name="state">The JToken to convert.</param>
    /// <returns>A Vector2 instance represented by the JToken.</returns>
    public static Vector2 ToVector2(this JToken state)
    {
        // Initialize a new Vector2 instance to populate
        Vector2 vector = new();

        // Ensure the JToken is a JObject with expected structure
        if (state is JObject jObject)
        {
            // Convert JObject to dictionary to facilitate property retrieval
            IDictionary<string, JToken> stateDictionary = jObject;

            // Retrieve and convert the "x" property if present
            if (stateDictionary.TryGetValue("x", out JToken x))
                vector.x = x.ToObject<float>();

            // Retrieve and convert the "y" property if present
            if (stateDictionary.TryGetValue("y", out JToken y))
                vector.y = y.ToObject<float>();
        }

        // Return the populated Vector2 instance
        return vector;
    }
}
