using UnityEngine;

[CreateAssetMenu(fileName = "MorphBall", menuName = "Abilities/MorphBall")]
public class MorphBall : Ability, IPerformable<bool>
{
    [SerializeField] private float waitToBall;

    private Animator _animator;
    private GameObject _standingGameObject;
    private GameObject _ballGameObject;
    private float _ballCounter;
    
    public bool MorphDown { get; set; }
    public bool MorphUp { get; set; }
    public bool IsActive => _ballGameObject.activeSelf;

    public GameObject StandingGameObject
    {
        set => _standingGameObject = value;
    }

    public GameObject BallGameObject
    {
        set => _ballGameObject = value;
    }

    private void Awake()
    {
        _ballCounter = waitToBall;
    }

    private void Morph(bool becomeBall)
    {
        if (becomeBall)
        {
            if (!_ballGameObject.activeSelf)
            {
                _ballCounter -= Time.deltaTime;

                if (_ballCounter <= 0)
                {
                    _ballGameObject.SetActive(true);
                    _standingGameObject.SetActive(false);
                }
            }
            else
            {
                _ballCounter = waitToBall;
            }
        }
        else
        {
            if (!_standingGameObject.activeSelf)
            {
                _ballCounter -= Time.deltaTime;

                if (_ballCounter <= 0)
                {
                    _standingGameObject.SetActive(true);
                    _ballGameObject.SetActive(false);
                }
            }
            else
            {
                _ballCounter = waitToBall;
            }
        }
    }
    
    public override bool Perform()
    {
        Morph(MorphDown);
        return true;
    }

    public override bool ResetAbility()
    {
        MorphDown = false;
        MorphUp = false;

        return base.ResetAbility();
    }

    public bool CanPerform(bool canMorph)
    {
        return Acquired;
    }
}
