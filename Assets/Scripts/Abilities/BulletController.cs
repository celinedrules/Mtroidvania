using UnityEngine;

public class BulletController : Weapon
{
    private bool _destroy;
    
    private void Update()
    {
        if (Rigidbody == null)
            return;

        Rigidbody.velocity = MoveDirection * moveSpeed;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
        Debug.Log("BULLET");
        AudioManager.Instance.PlayAudio(AudioType.BulletImpact);
    }
}