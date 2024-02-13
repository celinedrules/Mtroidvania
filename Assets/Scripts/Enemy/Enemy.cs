using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private bool isFreezable = true;
    [SerializeField] private int totalHealth;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private int damageAmount;
    [SerializeField] private bool destroyOnDamage;
    [SerializeField] private GameObject destroyEffect;

    private bool _isFrozen;
    
    public int Health => totalHealth;
    
    public void TakeDamage(int damageAmount)
    {
        totalHealth -= damageAmount;
        
        if (totalHealth <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioType.BulletImpact);
        }
    }

    public void Freeze()
    {
        if(!isFreezable)
            return;
        
        if(!_isFrozen)
        {
            AudioManager.Instance.PlayAudio(AudioType.EnemyFreeze, speed: 12.0f);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponentInChildren<Animator>().enabled = false;
            GetComponent<PaletteSwapper>()?.Freeze();
        }
        else
        {
            // TODO: Wait for EnemyFreeze to stop playing and play BuletImpact
        }
        
        _isFrozen = true;
    }
    
    private void DestroyEnemy()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, transform.rotation);
        
        Destroy(gameObject);
        
        AudioManager.Instance.PlayAudio(AudioType.EnemyExplode);
    }

    private void DealDamage()
    {
        //GameManager.Instance.DamagePlayer(damageAmount);
        PlayerHealth.Instance.DamagePlayer(damageAmount);
        
        if (destroyOnDamage)
        {
            if (deathEffect != null)
                Instantiate(destroyEffect, transform.position, transform.rotation);
            
            Destroy(gameObject);
            
            AudioManager.Instance.PlayAudio(AudioType.EnemyExplode);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            DealDamage();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            DealDamage();
    }
}
