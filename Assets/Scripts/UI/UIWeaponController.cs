using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponController : MonoBehaviour
{
    [BoxGroup("Missile"), SerializeField] private Image missileImage;
    [BoxGroup("Missile"), SerializeField] private Sprite missileInactive;
    [BoxGroup("Missile"), SerializeField] private Sprite missileActive;
    
    [BoxGroup("Super Missile"), SerializeField] private Image superMissileImage;
    [BoxGroup("Super Missile"), SerializeField] private Sprite superMissileInactive;
    [BoxGroup("Super Missile"), SerializeField] private Sprite superMissileActive;
    
    [BoxGroup("Power Bomb"), SerializeField] private Image powerBombImage;
    [BoxGroup("Power Bomb"), SerializeField] private Sprite powerBombInactive;
    [BoxGroup("Power Bomb"), SerializeField] private Sprite powerBombActive;
    
    [BoxGroup("Grapple Beam"), SerializeField] private Image grappleImage;
    [BoxGroup("Grapple Beam"), SerializeField] private Sprite grappleInactive;
    [BoxGroup("Grapple Beam"), SerializeField] private Sprite grappleActive;
    
    [BoxGroup("X-Ray Scope"), SerializeField] private Image xRayImage;
    [BoxGroup("X-Ray Scope"), SerializeField] private Sprite xRayInactive;
    [BoxGroup("X-Ray Scope"), SerializeField] private Sprite xRayActive;

    private void Start()
    {
        missileImage.enabled = false;
        superMissileImage.enabled = false;
        powerBombImage.enabled = false;
        grappleImage.enabled = false;
        xRayImage.enabled = false;
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
            case AbilityType.Missile:
                missileImage.enabled = true;
                break;
            case AbilityType.SuperMissile:
                superMissileImage.enabled = true;
                break;
            case AbilityType.PowerBomb:
                powerBombImage.enabled = true;
                break;
            case AbilityType.XRay:
                xRayImage.enabled = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    public void DeactivateWeapons()
    {
        grappleImage.sprite = grappleInactive;
        missileImage.sprite = missileInactive;
        superMissileImage.sprite = superMissileInactive;
        powerBombImage.sprite = powerBombInactive;
        xRayImage.sprite = xRayInactive;
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
            case AbilityType.Missile:
                missileImage.sprite = missileActive;
                break;
            case AbilityType.SuperMissile:
                superMissileImage.sprite = superMissileActive;
                break;
            case AbilityType.PowerBomb:
                powerBombImage.sprite = powerBombActive;
                break;
            case AbilityType.XRay:
                xRayImage.sprite = xRayActive;
                break;
            default:
                break;
        }
    }
}