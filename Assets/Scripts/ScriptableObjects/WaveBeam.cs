using UnityEngine;

[CreateAssetMenu(fileName = "WaveBeam", menuName = "Abilities/Wave Beam")]
public class WaveBeam : Ability
{
    [SerializeField] private float fireRate = 1.0f;
    
    private float _nextFireTime;

    public override bool ResetAbility()
    {
        _nextFireTime = 0.0f;

        return base.ResetAbility();
    }

    public bool CanPerform()
    {
        if (!(Time.time > _nextFireTime))
            return false;
        
        _nextFireTime = Time.time + fireRate;
        return true;

    }
}
