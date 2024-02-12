using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject bossToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        bossToActivate.SetActive(true);
        gameObject.SetActive(false);
    }
}
