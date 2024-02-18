using System;
using UnityEngine;

public class AbilityUnlock : Pickup
{
    [OnValueChanged("UpdateSprite")]
    [SerializeField] private AbilityType abilityToUnlock;

    public void UpdateSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(spriteRenderer == null)
            return;

        //Abilities abilities = LevelManager.Instance.Player.GetComponent<Abilities>();
        Abilities abilities = PlayerHealth.Instance.GetComponent<Abilities>();

        if (abilities == null)
            return;
        
        switch (abilityToUnlock)
        {
            case AbilityType.DoubleJump:
                spriteRenderer.sprite = abilities.DoubleJump.Sprite;
                break;
            case AbilityType.Dash:
                spriteRenderer.sprite = abilities.Dash.Sprite;
                break;
            case AbilityType.MorphBall:
                spriteRenderer.sprite = abilities.MorphBall.Sprite;
                break;
            case AbilityType.Bomb:
                spriteRenderer.sprite = abilities.Bomb.Sprite;
                break;
            case AbilityType.WaveBeam:
                spriteRenderer.sprite = abilities.WaveBeam.Sprite;
                break;
            case AbilityType.IceBeam:
                spriteRenderer.sprite = abilities.IceBeam.Sprite;
                break;
            case AbilityType.GrappleBeam:
                spriteRenderer.sprite = abilities.GrappleBeam.Sprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        Abilities abilities = other.GetComponentInParent<Abilities>();
        
        switch (abilityToUnlock)
        {
            case AbilityType.DoubleJump:
                abilities.DoubleJump.Acquired = true;
                UnlockMessage = abilities.DoubleJump.UnlockedText;
                break;
            case AbilityType.Dash:
                abilities.Dash.Acquired = true;
                UnlockMessage = abilities.Dash.UnlockedText;
                break;
            case AbilityType.MorphBall:
                abilities.MorphBall.Acquired = true;
                UnlockMessage = abilities.MorphBall.UnlockedText;
                break;
            case AbilityType.Bomb:
                abilities.Bomb.Acquired = true;
                UnlockMessage = abilities.Bomb.UnlockedText;
                break;
            case AbilityType.WaveBeam:
                abilities.WaveBeam.Acquired = true;
                UnlockMessage = abilities.WaveBeam.UnlockedText;
                break;
            case AbilityType.IceBeam:
                abilities.IceBeam.Acquired = true;
                UnlockMessage = abilities.IceBeam.UnlockedText;
                break;
            case AbilityType.GrappleBeam:
                abilities.GrappleBeam.Acquired = true;
                UIController.Instance.AcquireWeapon(AbilityType.GrappleBeam);
                UnlockMessage = abilities.GrappleBeam.UnlockedText;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        DisplayMessage();
    }
}