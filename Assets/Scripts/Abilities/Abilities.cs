using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public enum AbilityType
{
    DoubleJump,
    Dash,
    MorphBall,
    Bomb,
    WaveBeam,
    IceBeam,
    GrappleBeam,
    Missile,
    SuperMissile,
    PowerBomb,
    XRay
}

public class Abilities : MonoBehaviour, IJsonSavable
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

    //private bool skipReset;

    // public Abilities Init(PlayerController playerController)
    // {
    //     _playerController = playerController;
    //
    //     if(skipReset)
    //     {
    //         if (doubleJump != null)
    //         {
    //             doubleJump.Acquired = false;
    //             doubleJump.Animator = _playerController.Animator;
    //         }
    //
    //         if (dash != null)
    //         {
    //             dash.Acquired = false;
    //             dash.Animator = _playerController.Animator;
    //             dash.SpriteRenderer = _playerController.SpriteRenderer;
    //         }
    //
    //         if (morphBall != null)
    //         {
    //             morphBall.Acquired = false;
    //             morphBall.Animator = _playerController.Animator;
    //             morphBall.StandingGameObject = _playerController.StandingGameObject;
    //             morphBall.BallGameObject = _playerController.BallGameObject;
    //         }
    //
    //         if (bomb != null)
    //         {
    //             bomb.Acquired = false;
    //             bomb.ResetAbility();
    //         }
    //
    //         if (waveBeam != null)
    //         {
    //             waveBeam.Acquired = false;
    //             waveBeam.ResetAbility();
    //         }
    //
    //         if (iceBeam != null)
    //         {
    //             iceBeam.Acquired = false;
    //             iceBeam.ResetAbility();
    //         }
    //
    //         if (grappleBeam != null)
    //         {
    //             grappleBeam.Acquired = false;
    //             grappleBeam.ResetAbility();
    //         }
    //
    //         if (enableAllAbilities)
    //             EnableAbilities();
    //
    //         return this;
    //     }
    //     else
    //     {
    //         if (doubleJump != null)
    //         {
    //             doubleJump.Animator = _playerController.Animator;
    //         }
    //
    //         if (dash != null)
    //         {
    //             dash.Animator = _playerController.Animator;
    //             dash.SpriteRenderer = _playerController.SpriteRenderer;
    //         }
    //
    //         if (morphBall != null)
    //         {
    //             morphBall.Animator = _playerController.Animator;
    //             morphBall.StandingGameObject = _playerController.StandingGameObject;
    //             morphBall.BallGameObject = _playerController.BallGameObject;
    //         }
    //
    //         if (bomb != null)
    //         {
    //             bomb.ResetAbility();
    //         }
    //
    //         if (waveBeam != null)
    //         {
    //             waveBeam.ResetAbility();
    //         }
    //
    //         if (iceBeam != null)
    //         {
    //             iceBeam.ResetAbility();
    //         }
    //
    //         if (grappleBeam != null)
    //         {
    //             grappleBeam.ResetAbility();
    //         }
    //
    //         if (enableAllAbilities)
    //             EnableAbilities();
    //
    //         return this;
    //     }
    // }

    public Abilities Init(PlayerController playerController, bool skipReset = false)
    {
        _playerController = playerController;

        // Create a list of all abilities along with a lambda action to reset them if needed.
        var abilitiesWithResetActions = new List<(Ability ability, Action resetAction)>
        {
            (doubleJump, () => doubleJump?.ResetAbility()),
            (dash, () => dash?.ResetAbility()),
            (morphBall, () => morphBall?.ResetAbility()),
            (bomb, () => bomb?.ResetAbility()),
            (waveBeam, () => waveBeam?.ResetAbility()),
            (iceBeam, () => iceBeam?.ResetAbility()),
            (grappleBeam, () => grappleBeam?.ResetAbility())
        };

        foreach (var (ability, resetAction) in abilitiesWithResetActions)
        {
            if (ability == null)
                continue;
            
            // Common setup for all abilities.
            ability.Animator = _playerController.Animator;

            switch (ability)
            {
                // Specific setup for abilities that require it.
                case Dash dashAbility:
                    dashAbility.SpriteRenderer = _playerController.SpriteRenderer;
                    break;
                case MorphBall morphBallAbility:
                    morphBallAbility.StandingGameObject = _playerController.StandingGameObject;
                    morphBallAbility.BallGameObject = _playerController.BallGameObject;
                    break;
            }

            switch (GameManager.Instance.GameLoaded)
            {
                // Reset the ability if not skipping reset.
                case true:
                    resetAction.Invoke();
                    break;
                // Set 'Acquired' to false if skipping reset, except enable all abilities is true.
                case false when !enableAllAbilities:
                    ability.Acquired = false;
                    break;
            }
        }

        // Handle the enableAllAbilities flag.
        if (enableAllAbilities)
            EnableAbilities();

        // Remove acquired abilities from the scene
        
        
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
        Debug.Log("ALL");
        doubleJump.Acquired = true;
        dash.Acquired = true;
        morphBall.Acquired = true;
        bomb.Acquired = true;
        waveBeam.Acquired = true;
        iceBeam.Acquired = true;
        grappleBeam.Acquired = true;
    }

    public bool IsAbilityAcquired(AbilityType abilityType)
    {
        return abilityType switch
        {
            AbilityType.DoubleJump => doubleJump.Acquired,
            AbilityType.Dash => dash.Acquired,
            AbilityType.MorphBall => morphBall.Acquired,
            AbilityType.Bomb => bomb.Acquired,
            AbilityType.WaveBeam => waveBeam.Acquired,
            AbilityType.IceBeam => iceBeam.Acquired,
            AbilityType.GrappleBeam => grappleBeam.Acquired,
            _ => false
        };
    }
    
    public JToken CaptureAsJToken()
    {
        var abilitiesData = new JArray();
        abilitiesData.Add(new JObject { { "TypeName", nameof(DoubleJump) }, { "Acquired", doubleJump.Acquired } });
        abilitiesData.Add(new JObject { { "TypeName", nameof(Dash) }, { "Acquired", dash.Acquired } });
        abilitiesData.Add(new JObject { { "TypeName", nameof(MorphBall) }, { "Acquired", morphBall.Acquired } });
        abilitiesData.Add(new JObject { { "TypeName", nameof(Bomb) }, { "Acquired", bomb.Acquired } });
        abilitiesData.Add(new JObject { { "TypeName", nameof(WaveBeam) }, { "Acquired", waveBeam.Acquired } });
        abilitiesData.Add(new JObject { { "TypeName", nameof(IceBeam) }, { "Acquired", iceBeam.Acquired } });
        abilitiesData.Add(new JObject { { "TypeName", nameof(GrappleBeam) }, { "Acquired", grappleBeam.Acquired } });

        JObject state = new JObject
        {
            { "EnableAllAbilities", enableAllAbilities },
            { "Abilities", abilitiesData }
        };

        return state;
    }

    public void RestoreFromJToken(JToken state)
    {
        if (state is JObject jObject)
        {
            enableAllAbilities = jObject.Value<bool>("EnableAllAbilities");
            JArray abilitiesData = jObject.Value<JArray>("Abilities");

            foreach (JObject abilityData in abilitiesData)
            {
                string typeName = abilityData.Value<string>("TypeName");
                bool acquired = abilityData.Value<bool>("Acquired");
                
                switch (typeName)
                {
                    case nameof(DoubleJump):
                        doubleJump.Acquired = acquired;
                        break;
                    case nameof(Dash):
                        dash.Acquired = acquired;
                        break;
                    case nameof(MorphBall):
                        morphBall.Acquired = acquired;
                        break;
                    case nameof(Bomb):
                        bomb.Acquired = acquired;
                        break;
                    case nameof(WaveBeam):
                        waveBeam.Acquired = acquired;
                        break;
                    case nameof(IceBeam):
                        iceBeam.Acquired = acquired;
                        break;
                    case nameof(GrappleBeam):
                        grappleBeam.Acquired = acquired;
                        break;
                    
                }
            }

            if (enableAllAbilities)
                EnableAbilities();
        }
    }
}