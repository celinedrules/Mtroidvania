using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Animator animator;
    [SerializeField] private BulletController bullet;
    [SerializeField] private WaveBeamController waveBeam;
    [SerializeField] private IceBeamController iceBeam;
    [SerializeField] private GrappleBeamController grappleBeam;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject standingGameObject;
    [SerializeField] private GameObject ballGameObject;
    [SerializeField] private Animator standingAnimator;
    [SerializeField] private Animator ballAnimator;
    [SerializeField] private Transform bombPoint;


    private PlayerInputActions _playerControls;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _fire;
    private InputAction _dash;
    private InputAction _morphBall;
    private InputAction _switchWeapon;
    private Vector2 _moveDirection;
    private bool _isOnGround;

    private float _nextFireTime;

    private Abilities _abilities;
    [SerializeField] private WeaponType _currentWeapon;

    private static readonly int ShotFired = Animator.StringToHash("ShotFired");
    private static readonly int IsOnGround = Animator.StringToHash("IsOnGround");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private bool IsFlipped => transform.localScale.x == -1.0f;
    public Animator Animator => animator;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public GameObject StandingGameObject => standingGameObject;
    public GameObject BallGameObject => ballGameObject;
    public Transform BombPoint => bombPoint;

    // TODO: Maybeck if when CanMove = false, disable input instead
    public bool CanMove { get; set; }

    private void Awake()
    {
        // TODO: Maybe move this to an InputManager
        _playerControls = new PlayerInputActions();
        _abilities = GetComponent<Abilities>().Init(this);
        CanMove = true;
    }

    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.Enable();

        _jump = _playerControls.Player.Jump;
        _jump.Enable();
        _jump.performed += Jump;

        _switchWeapon = _playerControls.Player.SwitchWeapons;
        _switchWeapon.Enable();
        _switchWeapon.performed += SwitchWeapon;

        _fire = _playerControls.Player.Fire;
        _fire.Enable();
        _fire.performed += Fire;
        _fire.canceled += FireRelease;

        _dash = _playerControls.Player.Dash;
        _dash.Enable();
        _dash.performed += Dash;

        _morphBall = _playerControls.Player.MorphBall;
        _morphBall.Enable();
        _morphBall.started += ctx =>
        {
            if (ctx.ReadValue<Vector2>().y < 0)
                _abilities.Perform(AbilityType.MorphBall);
            else
                _abilities.Perform(AbilityType.MorphBall, false);
        };

        _morphBall.canceled += ctx =>
        {
            if (ctx.ReadValue<Vector2>().y == 0)
                _abilities.ResetAbility(AbilityType.MorphBall);
        };
    }

    private void OnDisable()
    {
        _move.Disable();
        _jump.Disable();
        _fire.Disable();
    }

    private void Update()
    {
        if (CanMove && !UIController.Instance.IsPaused)
        {
            _abilities.UpdateAbilities(AbilityType.Dash);
            _abilities.UpdateAbilities(AbilityType.MorphBall);

            if (!_abilities.IsDashing)
            {
                _moveDirection = _move.ReadValue<Vector2>();

                if (rigidBody.velocityX < 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (rigidBody.velocityX > 0)
                    transform.localScale = Vector3.one;
            }

            _isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }

        if (standingGameObject.activeSelf)
        {
            animator.SetBool(IsOnGround, _isOnGround);
            animator.SetFloat(Speed, Mathf.Abs(rigidBody.velocityX));
        }

        if (ballGameObject.activeSelf)
        {
            ballAnimator.SetFloat(Speed, Mathf.Abs(rigidBody.velocityX));
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = _abilities.IsDashing
            ? new Vector2(_abilities.DashSpeed * transform.localScale.x, rigidBody.velocityY)
            : new Vector2(_moveDirection.x * moveSpeed, rigidBody.velocityY);
    }

    public void EnableAnimations(bool enable)
    {
        if (enable)
        {
            standingAnimator.enabled = true;
            ballAnimator.enabled = true;
        }
        else
        {
            standingAnimator.enabled = false;
            ballAnimator.enabled = false;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        if (_abilities.IsBall)
            return;

        bool shouldJump = _isOnGround
            ? _abilities.ResetAbility(AbilityType.DoubleJump)
            : _abilities.Perform(AbilityType.DoubleJump, _isOnGround);

        if (shouldJump)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        if (_abilities.IsBall)
        {
            if (_abilities.Perform(AbilityType.Bomb))
                _abilities.UpdateAbilities(AbilityType.Bomb);

            return;
        }

        switch (_currentWeapon)
        {
            case WeaponType.Wave:
                waveBeam.Fire(shotPoint.position, IsFlipped);
                break;
            case WeaponType.Ice:
                iceBeam.Fire(shotPoint.position, IsFlipped);
                break;
            case WeaponType.Grapple:
                grappleBeam.Fire(shotPoint.position);
                break;
            case WeaponType.Standard:
                bullet.Fire(shotPoint.position, IsFlipped);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        animator.SetTrigger(ShotFired);
    }

    private void FireRelease(InputAction.CallbackContext context)
    {
        switch (_currentWeapon)
        {
            case WeaponType.Grapple:
                grappleBeam.Cancel();
                break;
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        _abilities.Perform(AbilityType.Dash, standingGameObject.activeSelf);
    }

    private void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (!CanMove)
            return;

        switch (_currentWeapon)
        {
            case WeaponType.Standard when _abilities.IceBeam.Acquired:
                _currentWeapon = WeaponType.Ice;
                break;
            case WeaponType.Standard when _abilities.WaveBeam.Acquired:
                _currentWeapon = WeaponType.Wave;
                break;
            case WeaponType.Standard when _abilities.GrappleBeam.Acquired:
                _currentWeapon = WeaponType.Grapple;
                UIController.Instance.ActivateWeapon(AbilityType.GrappleBeam);
                break;
            case WeaponType.Standard:
                _currentWeapon = WeaponType.Standard;
                UIController.Instance.DeactivateWeapons();
                break;
            case WeaponType.Ice when _abilities.WaveBeam.Acquired:
                _currentWeapon = WeaponType.Wave;
                break;
            case WeaponType.Ice:
            case WeaponType.Wave:
            case WeaponType.Missile:
            case WeaponType.SuperMissile:
            case WeaponType.PowerBomb:
            case WeaponType.Xray:
            case WeaponType.Grapple:
                _currentWeapon = WeaponType.Standard;
                UIController.Instance.DeactivateWeapons();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}