using UnityEngine;

public class PlayerState : Singleton<PlayerState>
{
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private int maxHealth;
    [SerializeField] private int doorId;
    
    public Vector3 PlayerPosition
    {
        get => playerPosition;
        set => playerPosition = value;
    }
    
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int DoorId
    {
        get => doorId;
        set => doorId = value;
    }
}
