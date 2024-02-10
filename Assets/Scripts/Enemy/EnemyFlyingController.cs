using UnityEditor;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    [SerializeField] private float chaseRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    private bool _isChasing;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private const float HomeOffset = 0.01f;
    private Transform _player;
    private Animator _animator;
    private static readonly int Chasing = Animator.StringToHash("Chasing");

    private void Start()
    {
        _player = PlayerHealth.Instance.transform;
        _animator = GetComponentInChildren<Animator>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _player.position) <= chaseRange)
        {
            if (!_isChasing)
            {
                _isChasing = true;
                _animator.SetBool(Chasing, _isChasing);
            }
            else if (_player.gameObject.activeSelf)
            {
                Vector3 direction = transform.position - _player.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                transform.position += -transform.right * (moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (_isChasing)
            {
                _isChasing = false;
                _animator.SetBool(Chasing, _isChasing);
            }
            else
            {
                if (Vector3.Distance(transform.position, _startPosition) > HomeOffset)
                {
                    Vector3 direction = transform.position - _startPosition;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    transform.rotation =
                        Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                    transform.position += -transform.right * (moveSpeed * Time.deltaTime);
                }
                else if (transform.rotation != _startRotation)
                {
                    transform.rotation = _startRotation;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = new Color(0, 1, 1, .05f);
        Handles.DrawSolidArc(transform.position, Vector3.forward, Vector3.up, 360, chaseRange);
    }
}