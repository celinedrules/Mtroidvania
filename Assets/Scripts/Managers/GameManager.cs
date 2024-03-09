using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Managers/Game Manager")]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SceneReference startingLevel;

    [OnValueChanged("UpdateAudioSettings", true)] [SerializeField]
    private bool muteAudio;

    private Dictionary<string, int> bossHealthStates = new Dictionary<string, int>();

    public bool GameLoaded { get; set; }
    public bool IsNewGame { get; set; } = true;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(SceneCompletelyLoaded());
    }

    private IEnumerator SceneCompletelyLoaded()
    {
        // Wait one frame so that all Start methods on scripts have been executed
        yield return null;

        if(!IsNewGame)
        {
            UpdateBossBattle();
        }
        Instance.GameLoaded = true;
    }

    private static void UpdateBossBattle()
    {
        BossBattle bossBattle = LevelManager.Instance.BossBattle;

        if (bossBattle != null)
        {
            JsonSavableEntity entity = bossBattle.GetComponent<JsonSavableEntity>();
            entity.RestoreFromJToken(JsonSavingSystem.Instance.GetSavedStateFor(entity));
            // bossBattle.RestoreFromJToken(
            //     JsonSavingSystem.Instance.GetSavedStateFor(bossBattle.GetComponent<JsonSavableEntity>()));
        }
    }

    private void Start()
    {
        UpdateAudioSettings();
    }

    public void LoadNewGame()
    {
        Load(startingLevel.BuildIndex);
    }

    public void Load(int sceneIndex)
    {
        PlayerState.Instance.DoorId = -1;
        SceneManager.LoadScene(sceneIndex);
    }

    private void UpdateAudioSettings()
    {
        AudioManager.Instance.Mute(muteAudio);
    }
}