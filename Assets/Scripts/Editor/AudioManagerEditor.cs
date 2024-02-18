using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Prepare the serialized object for editing
        
        AudioManager audioManager = (AudioManager)target;

        // Ensure the tracks array is always initialized with a size of 2.
        if (audioManager.Tracks == null || audioManager.Tracks.Length != 2)
        {
            audioManager.Tracks = new AudioManager.AudioTrack[2];
        }

        SerializedProperty tracksProperty = serializedObject.FindProperty("tracks");
        EditorGUILayout.LabelField("Audio Tracks");

        EditorGUI.indentLevel++;
        for (int i = 0; i < tracksProperty.arraySize; i++)
        {
            SerializedProperty trackProperty = tracksProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical("box");
            
            string customLabel = i == 0 ? "Background Music" : "Sound FX"; // Custom labels for each track
            EditorGUILayout.PropertyField(trackProperty.FindPropertyRelative("source"), new GUIContent($"{customLabel} Source"));

            // Add space here
            EditorGUILayout.Space(); // This adds a space below the source property field

            // Manually handle the audio array within each track
            SerializedProperty audioArray = trackProperty.FindPropertyRelative("audio");
            EditorGUI.indentLevel++;
            for (int j = 0; j < audioArray.arraySize; j++)
            {
                SerializedProperty audioObject = audioArray.GetArrayElementAtIndex(j);
                EditorGUILayout.BeginHorizontal(); // Start a horizontal group
                
                // Draw the type and clip properties on the same line
                EditorGUILayout.PropertyField(audioObject.FindPropertyRelative("type"), new GUIContent(), GUILayout.MaxWidth(200));
                EditorGUILayout.PropertyField(audioObject.FindPropertyRelative("clip"), new GUIContent(), GUILayout.ExpandWidth(true));

                // Add a remove button for each audio object
                if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(18)))
                {
                    audioArray.DeleteArrayElementAtIndex(j);
                }

                EditorGUILayout.EndHorizontal(); // End the horizontal group
            }

            EditorGUILayout.Space();
            
            // Add a button to add a new audio object
            if (GUILayout.Button("Add Audio Object"))
            {
                audioArray.InsertArrayElementAtIndex(audioArray.arraySize);
            }

            EditorGUI.indentLevel--;
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();

        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties(); // Apply the changes to the serialized object
    }
}
