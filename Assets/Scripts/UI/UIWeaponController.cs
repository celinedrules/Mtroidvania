using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponController : MonoBehaviour
{
    //[Header("Grapple Beam")]
    
    [BoxGroup("Grapple")] [SerializeField] private Image grappleImage;
    [BoxGroup("Grapple")] [SerializeField] private Sprite grappleInactive;
    [BoxGroup("Grapple")] [SerializeField] private Sprite grappleActive;
    
    [BoxGroup("Missle")] [SerializeField] private Sprite missleInactive;
    private Sprite missleActive;

    private void Start()
    {
        grappleImage.enabled = false;
    }

    public void AcquireWeapon(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.DoubleJump:
                break;
            case AbilityType.Dash:
                break;
            case AbilityType.MorphBall:
                break;
            case AbilityType.Bomb:
                break;
            case AbilityType.WaveBeam:
                break;
            case AbilityType.IceBeam:
                break;
            case AbilityType.GrappleBeam:
                grappleImage.enabled = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    public void DeactivateWeapons()
    {
        grappleImage.sprite = grappleInactive;
    }

    public void ActivateWeapon(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.DoubleJump:
                break;
            case AbilityType.Dash:
                break;
            case AbilityType.MorphBall:
                break;
            case AbilityType.Bomb:
                break;
            case AbilityType.WaveBeam:
                break;
            case AbilityType.IceBeam:
                break;
            case AbilityType.GrappleBeam:
                grappleImage.sprite = grappleActive;
                break;
            default:
                break;
        }
    }
}