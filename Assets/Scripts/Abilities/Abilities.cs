using System;
using UnityEngine;

public enum AbilityType
{
    DoubleJump,
    Dash,
    MorphBall,
    Bomb,
    WaveBeam,
    IceBeam,
    GrappleBeam
}

public class Abilities : MonoBehaviour
{
    [SerializeField] private bool enableAllAbilities;
    [SerializeField] private DoubleJump doubleJump;
    [SerializeField] private Dash dash;
    [SerializeField] private MorphBall morphBall;
    [SerializeField] private Bomb bomb;
    [SerializeField] private WaveBeam waveBeam;
    [SerializeField] private IceBeam iceBeam;
    [SerializeField] private GrappleBeam grappleBeam;

    private PlayerController _playerController;

    public bool EnableAllAbilities
    {
        get => enableAllAbilities;
        set => enableAllAbilities = value;
    }
    public DoubleJump DoubleJump => doubleJump;
    public Dash Dash => dash;
    public MorphBall MorphBall => morphBall;
    public Bomb Bomb => bomb;
    public WaveBeam WaveBeam => waveBeam;
    public IceBeam IceBeam => iceBeam;
    public GrappleBeam GrappleBeam => grappleBeam;
    public bool IsDashing => dash.IsDashing;
    public float DashSpeed => dash.DashSpeed;
    public bool IsBall => morphBall.IsActive;
    

    public Abilities Init(PlayerController playerController)
    {
        _playerController = playerController; 

        if (doubleJump != null)
        {
            doubleJump.Acquired = false;
            doubleJump.Animator = _playerController.Animator;
        }

        if (dash != null)
        {
            dash.Acquired = false;
            dash.Animator = _playerController.Animator;
            dash.SpriteRenderer = _playerController.SpriteRenderer;
        }

        if (morphBall != null)
        {
            morphBall.Acquired = false;
            morphBall.Animator = _playerController.Animator;
            morphBall.StandingGameObject = _playerController.StandingGameObject;
            morphBall.BallGameObject = _playerController.BallGameObject;
        }

        if (bomb != null)
        {
            bomb.Acquired = false;
            bomb.ResetAbility();
        }

        if (waveBeam != null)
        {
            waveBeam.Acquired = false;
            waveBeam.ResetAbility();
        }

        if (iceBeam != null)
        {
            iceBeam.Acquired = false;
            iceBeam.ResetAbility();
        }

        if (grappleBeam != null)
        {
            grappleBeam.Acquired = false;
            grappleBeam.ResetAbility();
        }
        
        if (enableAllAbilities)
            EnableAbilities();
        
        return this;
    }

    public void UpdateSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        if (dash != null)
            dash.SpriteRenderer = spriteRenderer;
    }

    public void UpdateAbilities(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.DoubleJump:
                break;
            case AbilityType.Dash:
                if (dash != null)
                    dash.UpdateDash(transform);
                break;
            case AbilityType.MorphBall:
                if (morphBall != null && (morphBall.MorphDown || morphBall.MorphUp))
                    morphBall.Perform();
                break;
            case AbilityType.Bomb:
                if (bomb != null)
                    bomb.Deploy(_playerController.BombPoint);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    public bool Perform(AbilityType abilityType, bool canPerform = true)
    {
        switch (abilityType)
        {
            case AbilityType.DoubleJump:
                if (doubleJump != null && doubleJump.CanPerform(canPerform))
                    return doubleJump.Perform();
                break;
            case AbilityType.Dash:
                if (dash != null && dash.CanPerform(canPerform))
                    return dash.Perform();
                break;
            case AbilityType.MorphBall:
                if (morphBall != null && morphBall.CanPerform(canPerform))
                {
                    morphBall.MorphDown = canPerform;
                    morphBall.MorphUp = !canPerform;
                    return morphBall.Perform();
                }
                break;
            case AbilityType.Bomb:
                if (bomb != null && bomb.CanPerform(canPerform))
                    return bomb.Perform();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }

        return false;
    }

    public bool ResetAbility(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.DoubleJump:
                if (doubleJump != null)
                    return doubleJump.ResetAbility();
                break;
            case AbilityType.Dash:
                break;
            case AbilityType.MorphBall:
                if (morphBall != null)
                    return morphBall.ResetAbility();
                break;
            case AbilityType.WaveBeam:
                if (waveBeam != null)
                    return waveBeam.ResetAbility();
                break;
            case AbilityType.IceBeam:
                if (iceBeam != null)
                    return iceBeam.ResetAbility();
                break;
            case AbilityType.GrappleBeam:
                if (grappleBeam != null)
                    return grappleBeam.ResetAbility();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }

        return false;
    }

    private void EnableAbilities()
    {
        doubleJump.Acquired = true;
        dash.Acquired = true;
        morphBall.Acquired = true;
        bomb.Acquired = true;
        waveBeam.Acquired = true;
        iceBeam.Acquired = true;
        grappleBeam.Acquired = true;
    }
}