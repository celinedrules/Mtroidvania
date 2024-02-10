using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Abilities/Bomb")]
public class Bomb : Ability, IPerformable<bool>
{
    [SerializeField] private GameObject bomb;
    [SerializeField] private int maxBombs;
    
    private int _numDeployedBombs;
    private bool _deployed;

    public override bool ResetAbility()
    {
        _numDeployedBombs = 0;
        return base.ResetAbility();
    }

    public void Deploy(Transform transform)
    {
        if (!_deployed)
            return;

        Instantiate(bomb, transform.position, transform.rotation);
        
        _numDeployedBombs++;
        _deployed = false;
    }
    
    public override bool Perform()
    {
        base.Perform();

        if(_numDeployedBombs < maxBombs)
            return _deployed = true;
        
        return false;
    }

    public bool CanPerform(bool canPerform) => Acquired && canPerform;
    [UsedImplicitly] public void Destroy() => _numDeployedBombs--;
}