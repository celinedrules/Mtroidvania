// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class LevelManager : Singleton<LevelManager>
// {
//     [SerializeField] private PlayerController playerPrefab;
//     [SerializeField] private float respawnDelay = 2.0f;
//     //[SerializeField] private CheckPoint initialSpawnPoint;
//
//     public Vector3 InitialSpawnPointPosition { get; private set; }
//     
//     public PlayerController Player { get; private set; }
//
//     protected override void Awake()
//     {
//         base.Awake();
//         //InitialSpawnPointPosition = (initialSpawnPoint == null) ? Vector3.zero : initialSpawnPoint.transform.position;
//         InitialSpawnPointPosition = new Vector3(-7.5f, -9.5f);
//
//     }
//
//     private void Start()
//     {
//         InstantiatePlayer();
//     }
//
//     private void InstantiatePlayer()
//     {
//         // PlayerController newPlayer;
//         //
//         // if (GameManager.Instance.PersistentPlayer != null)
//         // {
//         //     Player = GameManager.Instance.PersistentPlayer;
//         //     return;
//         // }
//         //
//         // if (GameManager.Instance.StoredPlayer != null)
//         // {
//         //     newPlayer = Instantiate(GameManager.Instance.StoredPlayer, InitialSpawnPointPosition,
//         //         Quaternion.identity);
//         //     newPlayer.name = GameManager.Instance.StoredPlayer.name;
//         //     Player = newPlayer;
//         //     return;
//         // }
//         //
//         // if(playerPrefab == null)
//         //     return;
//         //
//         // newPlayer = Instantiate(playerPrefab, InitialSpawnPointPosition, Quaternion.identity);
//         // newPlayer.name = playerPrefab.name;
//         // Player = newPlayer;
//     }
//     
//     public void Respawn()
//     {
//         StartCoroutine(RespawnCo());
//     }
//
//     protected IEnumerator RespawnCo()
//     {
//         if(playerPrefab == null)
//              yield break;
//         
//         yield return new WaitForSeconds(respawnDelay);
//
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//
//         Player.transform.position = InitialSpawnPointPosition;
//         Player.gameObject.SetActive(true);
//         Debug.Log("TODO");
//         //PlayerHealth.Instance.FillHealth();
//
//         // if (currentCheckpoint == null)
//         //     currentCheckpoint = initialSpawnPoint;
//
//         if (Player == null)
//              InstantiatePlayer();
//
//         // if (currentCheckpoint != null)
//         //     currentCheckpoint.SpawnPlayer(Player);
//         // else
//         //     Debug.LogWarning("LevelManager : no checkpoint or initial spawn point has been defined, can't respawn the Player.");
//     }
// }