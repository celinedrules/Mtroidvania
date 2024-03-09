using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Abilities/Dash")]
public class Dash : Ability, IPerformable<bool>
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private SpriteRenderer afterImage;
    [SerializeField] private float afterImageLifeTime;
    [SerializeField] private float timeBetweenAfterImages;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float waitAfterDashing;

    private Transform _transform;

    private bool _isDashing;
    private float _dashCounter;
    private float _afterImageCounter;
    private float _dashRechargeCounter;

    public bool IsDashing => _dashCounter > 0;

    public float DashSpeed => dashSpeed;

    public SpriteRenderer SpriteRenderer { get; set; }

    public void UpdateDash(Transform transform)
    {
        _transform = transform;

        if (_dashRechargeCounter > 0)
            _dashRechargeCounter -= Time.deltaTime;

        if (IsDashing)
        {
            _dashCounter -= Time.deltaTime;
            _afterImageCounter -= Time.deltaTime;
            
            AudioManager.Instance.PlayAudio(AudioType.PlayerDash);

            if (_afterImageCounter <= 0)
                ShowAfterImage();

            _dashRechargeCounter = waitAfterDashing;
        }
    }

    private void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, _transform.position, _transform.rotation);
        image.sprite = SpriteRenderer.sprite;
        image.transform.localScale = _transform.localScale;
        image.color = afterImageColor;
        Destroy(image.gameObject, afterImageLifeTime);

        _afterImageCounter = timeBetweenAfterImages;
    }

    public override bool Perform()
    {
        if (_dashRechargeCounter <= 0)
        {
            _dashCounter = dashTime;
            ShowAfterImage();

            return true;
        }

        return false;
    }

    public bool CanPerform(bool canPerform)
    {
        return Acquired && canPerform;
    }
}