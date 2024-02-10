using System;
using System.Collections;
using UnityEngine;

public class IceBeamController : Weapon
{
    [SerializeField] private ParticleSystem particleSystem;
    private bool _destroy;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ParticleSystem());
    }

    private void Update()
    {
        if(Rigidbody == null)
            return;

        Rigidbody.velocity = MoveDirection * moveSpeed;
    }

    public override void Fire(Vector3 position, bool flipX)
    {
        const float verticalOffset = 1.0f;

        InstantiateProjectile(this, position, flipX);
        InstantiateProjectile(this, position + new Vector3(0, verticalOffset, 0), flipX);
        InstantiateProjectile(this, position - new Vector3(0, verticalOffset, 0), flipX);
    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damageAmount);
            enemy.Freeze();
            
            if (enemy.Health <= 0)
                AudioManager.Instance.PlayAudio(AudioType.EnemyExplode);
        }
        
        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private IEnumerator ParticleSystem()
    {
        while (true)
        {
            particleSystem.Play();
            yield return new WaitForSeconds(particleSystem.main.duration + 0.5f);
            particleSystem.Stop();
        }
    }
}
