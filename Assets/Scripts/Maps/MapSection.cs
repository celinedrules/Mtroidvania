using UnityEngine;

public class MapSection : MonoBehaviour
{
    [SerializeField] private SpriteMask borderMask;
    [SerializeField] private GameObject playerMapSprite;

    private bool _touchedByPlayer;

    private void Awake()
    {
        playerMapSprite = GameObject.FindWithTag("PlayerMapSprite");
    }

    private void Activate()
    {
        if (borderMask == null)
            borderMask = GetComponentInChildren<SpriteMask>();
        
        borderMask.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerMapCollision"))
            return;
        
        Activate();

        if (playerMapSprite != null)
            playerMapSprite.transform.position = transform.position;
    }
}