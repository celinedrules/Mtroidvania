using System;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float flashTime;
    [SerializeField] private SpriteRenderer[] playerSprites;

    private int _currentHealth;
    private float _invincibilityCounter;
    private float _flashCounter;

    public int MaxHealth => maxHealth;

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    private void Start()
    {
        transform.position = PlayerState.Instance.PlayerPosition;
        CurrentHealth = PlayerState.Instance.MaxHealth = maxHealth;
        UIController.Instance.SetupPlayerHealth(maxHealth);
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
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
            {
                AudioManager.Instance.PlayAudio(AudioType.PlayerDeath);
                CurrentHealth = 0;
                gameObject.SetActive(false);
                RespawnController.Instance.Respawn();
            }
            else
            {
                _invincibilityCounter = invincibilityTime;
            }
        }

        UIController.Instance.UpdatePlayerHealth(_currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
        
        UIController.Instance.UpdatePlayerHealth(CurrentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        CurrentHealth = maxHealth;
        UIController.Instance.AddHealthTank(CurrentHealth, maxHealth);
    }
    
    public void FillHealth()
    {
        CurrentHealth = maxHealth;
        UIController.Instance.UpdatePlayerHealth(CurrentHealth, maxHealth);
    }
}