using System;
using UnityEngine;

public class Respawn : Singleton<Respawn>
{
    [SerializeField] private float waitToRespawn;
    
    private Vector3 _respawnPoint;
    private GameObject _player;

    private void Start()
    {
        Debug.Log("TODO");
        //_player = PlayerHealth.Instance.gameObject;
        _respawnPoint = _player.transform.position;
    }

    public void RespawnPlayer()
    {
        
    }
}
