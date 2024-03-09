using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RespawnController : Singleton<RespawnController>
{
    [SerializeField] private GameObject playerDeathEffect;
    [SerializeField] private float waitToRespawn = 2.0f;

    private Vector3 respawnPoint;

    public void SetSpawn(Vector3 spawnPoint)
    {
        PlayerState.Instance.PlayerPosition = spawnPoint;
    }

    public void Respawn() => StartCoroutine(RespawnCo());

    private IEnumerator RespawnCo()
    {
        Transform t = PlayerHealth.Instance.GetComponent<PlayerController>().transform;

        if (playerDeathEffect != null)
            Instantiate(playerDeathEffect, t.position, t.rotation);
        
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
        if (!GameManager.Instance.GameLoaded)
        {
            GameObject startingPoint = GameObject.FindWithTag("Player Starting Point");

            if (startingPoint)
            {
                Instance.SetSpawn(startingPoint.transform.position);
                return;
            }

            if (EditorUtility.DisplayDialog("Error: No starting point found",
                    "No starting point was found in scene '" + scene.name +
                    "'. The starting point must use the tag 'Player Starting Point'", "Quit"))
            {
                EditorApplication.isPlaying = false;
            }
        }

        DoorController[] doors = FindObjectsByType<DoorController>(FindObjectsSortMode.None);

        foreach (DoorController door in doors)
        {
            if (door.DoorId == PlayerState.Instance.DoorId)
            {
                Instance.SetSpawn(door.SpawnPoint.transform.position);
            }
        }
    }
}