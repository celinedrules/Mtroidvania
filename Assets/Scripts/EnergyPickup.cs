using System;
using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    [SerializeField] private int energyAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;

        AudioManager.Instance.PlayAudio(AudioType.PickupGem);
        PlayerHealth.Instance.Heal(energyAmount);
        Destroy(gameObject);
    }
}
