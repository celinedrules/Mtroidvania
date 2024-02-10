using System;
using UnityEngine;

public class WaveBeamController : Weapon
{
    public float frequency = 10f;
    public float magnitude = 0.5f;
    
    private float _wavePhase;

    protected override void Start()
    {
        base.Start();
        _wavePhase = MathF.PI / 2;
    }
    
    private void FixedUpdate()
    {
        if(Rigidbody == null)
            return;
        
        // Calculate the wave pattern
        _wavePhase += frequency * Time.fixedDeltaTime;
        float wave = Mathf.Sin(_wavePhase) * magnitude;

        // Apply the forward movement along the x-axis and wave motion along the y-axis
        Vector2 velocity = new Vector2(moveSpeed * MoveDirection.x, wave);
        Rigidbody.velocity = velocity;
    }
}
