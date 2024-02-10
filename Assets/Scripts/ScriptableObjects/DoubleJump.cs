using UnityEngine;

[CreateAssetMenu(fileName = "DoubleJump", menuName = "Abilities/Double Jump")]
public class DoubleJump : Ability, IPerformable<bool>
{
    private static readonly int Jump = Animator.StringToHash("DoubleJump");

    public override bool Perform()
    {
        base.Perform();
        Animator.SetTrigger(Jump);
        return true;
    }

    public bool CanPerform(bool isOnGround)
    {
        if (Acquired)
            return !isOnGround && CanTrigger;

        return false;
    }
}