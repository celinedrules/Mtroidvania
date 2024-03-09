using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject bossToActivate;

    private void Start()
    {
        if(LevelManager.Instance.BossDead)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        bossToActivate.SetActive(true);
    }

    // public JToken CaptureAsJToken()
    // {
    //     if (GetComponent<BossHealth>() != null)
    //         return JToken.FromObject(GetComponent<BossHealth>());
    //     else
    //         return 0;
    // }
    //
    // public void RestoreFromJToken(JToken state)
    // {
    //     int bossHealth = state["BossActivator"].ToObject<int>();
    //     bossToActivate.GetComponent<BossHealth>().CurrentHealth = bossHealth;
    //
    //     if (bossHealth < 10)
    //     {
    //         DestroyImmediate(bossToActivate);
    //         gameObject.SetActive(false);
    //     }
    // }
}
