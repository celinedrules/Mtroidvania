using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Managers/Level Manager")]
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private BossBattle bossBattle;
    [SerializeField] private bool bossDead;
    
    public bool BossDead
    {
        get => bossDead;
        set => bossDead = value;
    }

    public BossBattle BossBattle => bossBattle;
    
    private void Start()
    {
        if(!AudioManager.Instance.IsAudioPlaying(AudioType.LevelMusic))
            AudioManager.Instance.PlayAudio(AudioType.LevelMusic);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        DisableAcquiredAbilities();
        DisableBoss();
    }

    private void DisableBoss()
    {
        //bossDead = true;
        //Destroy(bossBattle);
        //bossBattle.gameObject.SetActive(false);
    }

    private void DisableAcquiredAbilities()
    {
        var abilities = PlayerHealth.Instance.GetComponent<Abilities>();
        
        // FindObjectsByType<AbilityUnlock>(FindObjectsSortMode.None)
        //     .Where(unlock => abilities.IsAbilityAcquired(unlock.AbilityToUnlock))
        //     .ToList()
        //     .ForEach(unlock => unlock.gameObject.SetActive(false));
        
        FindObjectsByType<AbilityUnlock>(FindObjectsSortMode.None)
            .Where(unlock => abilities.IsAbilityAcquired(unlock.AbilityToUnlock))
            .ToList()
            .ForEach(unlock =>Destroy(unlock.gameObject));
    }
}