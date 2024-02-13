using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Singleton<BossHealth>
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private int currentHealth = 30;
    [SerializeField] private BossBattle boss;

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    private void Start()
    {
        healthSlider.maxValue = CurrentHealth;
        healthSlider.value = CurrentHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            //AudioManager.Instance.PlayAudio(AudioType.EnemyExplode);
            CurrentHealth = 0;
            boss.EndBattle();
        }

        //AudioManager.Instance.PlayAudio(AudioType.BulletImpact);
        healthSlider.value = CurrentHealth;
    }
}