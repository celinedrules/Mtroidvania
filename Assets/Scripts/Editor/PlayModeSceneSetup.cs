using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

[InitializeOnLoad]
public class PlayModeSceneManagement
{
    // Keep track of scenes that were unloaded
    private static List<string> unloadedScenePaths = new List<string>();

    static PlayModeSceneManagement()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                UnloadNonActiveScenes();
                break;
            case PlayModeStateChange.EnteredEditMode:
                ReloadUnloadedScenes();
                break;
        }
    }

    private static void UnloadNonActiveScenes()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != activeScene && scene.isLoaded)
            {
                // Store the path before unloading
                unloadedScenePaths.Add(scene.path);
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }

    private static void ReloadUnloadedScenes()
    {
        foreach (string scenePath in unloadedScenePaths)
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
        }
        // Clear the list after reloading
        unloadedScenePaths.Clear();
    }
}