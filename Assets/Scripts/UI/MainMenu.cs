using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneReference sceneToLoad;
    [SerializeField] private GameObject continueButton;

    private string _saveFile;

    private void Start()
    {
        _saveFile = CheckIfSaveFileExists();
        
        AudioManager.Instance.PlayAudio(AudioType.MainMenu);
    }

    private string CheckIfSaveFileExists()
    {
        if (JsonSavingSystem.Instance.ListSaves().Any())
        {
            continueButton.SetActive(true);
            continueButton.GetComponentInChildren<Button>().Select();
            return JsonSavingSystem.Instance.ListSaves().FirstOrDefault();
        }
        else
        {
            continueButton.SetActive(false);
        }

        return "";
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.LoadNewGame();
        AudioManager.Instance.StopAudio(AudioType.MainMenu);
    }

    public void Continue()
    {
        //JsonSavingSystem.Instance.Load(_saveFile);
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