using UnityEngine;

public class BombController : Weapon
{
    [SerializeField] private float timeToExplode = 1.0f;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float blastRange;
    [SerializeField] private float blastPower;
    [SerializeField] private LayerMask interactableLayerMask;
    
    private void Update()
    {
        timeToExplode -= Time.deltaTime;

        if (!(timeToExplode <= 0))
            return;
        
        if (explosion)
            Instantiate(explosion, transform.position, transform.rotation);
        
        Destroy(gameObject);

        Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, interactableLayerMask);

        foreach (Collider2D col in objectsToRemove)
        {
            GameLayer layer = Utils.LayerNameToEnum(LayerMask.LayerToName(col.gameObject.layer));
            
            if(layer == GameLayer.Destructible)
                Destroy(col.gameObject);
            
            if(layer == GameLayer.Enemy)
                col.GetComponent<Enemy>()?.TakeDamage(damageAmount);

            if (layer != GameLayer.Player)
                continue;

            Rigidbody2D rigidBody = col.gameObject.GetComponentInParent<Rigidbody2D>();
            
            if (rigidBody)
                rigidBody.velocityY = blastPower;
        }
    }
}
