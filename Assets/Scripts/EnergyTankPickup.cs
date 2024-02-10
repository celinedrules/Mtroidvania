using System;
using UnityEngine;

public class EnergyTankPickup : Pickup
{
    private void Start()
    {
        UnlockMessage = "Energy Reserve";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;

        DisplayMessage();
        
        PlayerHealth.Instance.IncreaseMaxHealth(100);
    }
}
