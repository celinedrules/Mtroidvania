using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>, IJsonSavable
{
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float flashTime;
    [SerializeField] private SpriteRenderer[] playerSprites;

    private float _invincibilityCounter;
    private float _flashCounter;

    private void Start()
    {
        transform.position = PlayerState.Instance.PlayerPosition;
        
        if(!GameManager.Instance.GameLoaded)
        {
            PlayerState.Instance.CurrentHealth = PlayerState.Instance.MaxHealth;
            UIController.Instance.SetupPlayerHealth(PlayerState.Instance.CurrentHealth);
        }
        else
        {
            UIController.Instance.SetupPlayerHealth(PlayerState.Instance.CurrentHealth);
        }
    }

    private void Update()
    {
        if (_invincibilityCounter > 0)
        {
            _invincibilityCounter -= Time.deltaTime;
            _flashCounter -= Time.deltaTime;

            if (_flashCounter <= 0)
            {
                foreach (SpriteRenderer sprite in playerSprites)
                    sprite.enabled = !sprite.enabled;

                _flashCounter = flashTime;
            }

            if (_invincibilityCounter <= 0)
            {
                foreach (SpriteRenderer sprite in playerSprites)
                    sprite.enabled = true;

                _flashCounter = 0;
            }
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (_invincibilityCounter <= 0)
        {
            PlayerState.Instance.CurrentHealth -= damageAmount;

            if (PlayerState.Instance.CurrentHealth <= 0)
            {
                AudioManager.Instance.PlayAudio(AudioType.PlayerDeath);
                PlayerState.Instance.CurrentHealth = 0;
                gameObject.SetActive(false);
                RespawnController.Instance.Respawn();
            }
            else
            {
                AudioManager.Instance.PlayAudio(AudioType.PlayerHurt);
                _invincibilityCounter = invincibilityTime;
            }
        }

        UIController.Instance.UpdatePlayerHealth(PlayerState.Instance.CurrentHealth, PlayerState.Instance.MaxHealth);
    }

    public void Heal(int amount)
    {
        PlayerState.Instance.CurrentHealth += amount;

        if (PlayerState.Instance.CurrentHealth > PlayerState.Instance.MaxHealth)
            PlayerState.Instance.CurrentHealth = PlayerState.Instance.MaxHealth;
        
        UIController.Instance.UpdatePlayerHealth(PlayerState.Instance.CurrentHealth, PlayerState.Instance.MaxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        PlayerState.Instance.MaxHealth += amount;
        PlayerState.Instance.CurrentHealth = PlayerState.Instance.MaxHealth;
        UIController.Instance.AddHealthTank(PlayerState.Instance.CurrentHealth, PlayerState.Instance.MaxHealth);
    }
    
    public void FillHealth()
    {
        PlayerState.Instance.CurrentHealth = PlayerState.Instance.MaxHealth;
        UIController.Instance.UpdatePlayerHealth(PlayerState.Instance.CurrentHealth, PlayerState.Instance.MaxHealth);
    }

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(new PlayerSaveData()
        {
            Position = transform.position .ToToken(),
            CurrentHealth = JToken.FromObject(PlayerState.Instance.CurrentHealth)
        });
    }

    public void RestoreFromJToken(JToken state)
    {
        PlayerSaveData data = state.ToObject<PlayerSaveData>();
        PlayerState.Instance.PlayerPosition = data.Position.ToVector2();
        PlayerState.Instance.CurrentHealth = data.CurrentHealth.ToObject<int>();
    }
    
    [Serializable]
    private struct PlayerSaveData
    {
        public JToken Position;
        public JToken CurrentHealth;
    }
}