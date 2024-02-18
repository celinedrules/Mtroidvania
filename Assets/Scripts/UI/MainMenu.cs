using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneReference sceneToLoad;

    private void Start()
    {
        AudioManager.Instance.PlayAudio(AudioType.MainMenu);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(sceneToLoad);
        AudioManager.Instance.StopAudio(AudioType.MainMenu);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}