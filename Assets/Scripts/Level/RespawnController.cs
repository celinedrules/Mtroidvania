using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : Singleton<RespawnController>
{
    [SerializeField] private GameObject playerDeathEffect;
    [SerializeField] private float waitToRespawn = 2.0f;

    private Vector3 respawnPoint;
    
    public void SetSpawn(Vector3 spawnPoint) => PlayerState.Instance.PlayerPosition = spawnPoint;
    public void Respawn() => StartCoroutine(RespawnCo());

    private IEnumerator RespawnCo()
    {
        Transform t = PlayerHealth.Instance.GetComponent<PlayerController>().transform;
        
        if (playerDeathEffect != null)
            Instantiate(playerDeathEffect, t.position, t.rotation);

        PlayerState.Instance.MaxHealth = PlayerHealth.Instance.MaxHealth;
        yield return new WaitForSeconds(waitToRespawn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerHealth.Instance.FillHealth();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject startingPoint = GameObject.FindWithTag("Player Starting Point");

        if (startingPoint)
        {
            Instance.SetSpawn(startingPoint.transform.position);
            return;
        }

        DoorController[] doors = FindObjectsByType<DoorController>(FindObjectsSortMode.None);

        foreach (DoorController door in doors)
        {
            if(door.DoorId == PlayerState.Instance.DoorId)
            {
                Instance.SetSpawn(door.SpawnPoint.transform.position);
            }
        }
    }
}
