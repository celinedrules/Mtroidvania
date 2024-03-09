using System;
using UnityEngine;

public class BossBullet : Weapon
{
    protected override void Start()
    {
        base.Start();

        Vector3 direction = transform.position - PlayerHealth.Instance.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        AudioManager.Instance.PlayAudio(AudioType.BossShot);
    }

    private void Update()
    {
        Rigidbody.velocity = -transform.right * moveSpeed;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            PlayerHealth.Instance.DamagePlayer(damageAmount);
        
        if(impactEffect == null)
            return;

        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        
        AudioManager.Instance.PlayAudio(AudioType.BulletImpact);
    }
}
