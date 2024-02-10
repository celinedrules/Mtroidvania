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
}