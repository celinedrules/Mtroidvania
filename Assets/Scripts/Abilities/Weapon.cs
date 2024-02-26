using System;using UnityEngine;

public enum WeaponType
{
    Standard,
    // Charge
    Ice,
    Wave,
    Grapple,
    Missile,
    SuperMissile,
    PowerBomb,
    Xray
    // Spazer,
    // Plasma
}

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] protected GameObject impactEffect;
    [SerializeField] protected int damageAmount;
    [SerializeField] protected float fireRate = 1.0f;
    //[SerializeField] protected Transform shotPoint;

    protected Rigidbody2D Rigidbody;
    protected bool IsVisible;
    public float FireRate => fireRate;
    
    public Vector2 MoveDirection
    {
        get => moveDirection;
        set => moveDirection = value;
    }
    
    protected virtual void Start() => Rigidbody = GetComponent<Rigidbody2D>();

    public virtual void Fire(Vector3 position, bool flipX = false)
    {
        InstantiateProjectile(this, position, flipX);
    }
    
    protected void InstantiateProjectile(Weapon projectile, Vector3 shotPoint, bool flipX)
    {
        Vector2 direction = flipX ? new Vector2(-1, 0) : new Vector2(1, 0);
        Instantiate(projectile, shotPoint, Quaternion.identity).MoveDirection = direction;
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<Enemy>().TakeDamage(damageAmount);

        if (other.CompareTag("Boss"))
            BossHealth.Instance.TakeDamage(damageAmount);

        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
    
    protected void OnBecameInvisible()
    {
        if (!IsVisible)
            return;

        IsVisible = false;
        Destroy(gameObject);
    }

    protected void OnBecameVisible()
    {
        if (IsVisible)
            return;

        IsVisible = true;
    }
}
