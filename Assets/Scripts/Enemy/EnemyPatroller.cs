using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    [SerializeField] private PatrolPoints patrolPoints; // Reference to the PatrolPoints script
    [SerializeField] private bool showPath;
    [SerializeField] private bool reverse; // Allow the enemy to keep looping along the path
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitAtPoints;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallCheckDistance = 0.5f; // How far ahead of the enemy to check for walls

    private int _currentPoint;
    private float _waitCounter;
    private bool _isInReverse; // Is the enemy going down the path in reverse
    private bool _canMove; // Can the enemy even move
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    public bool ShowPath => showPath;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        // Used to prevent the patrol points from moving with the enemy
        patrolPoints.gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        _waitCounter = waitAtPoints;
        _canMove = true;
    }

    private void Update()
    {
        if (_canMove)
        {
            // Get the direction the enemy is facing
            Vector2 direction = transform.position.x < patrolPoints.Points[_currentPoint].position.x
                ? Vector2.right
                : Vector2.left;

            // Check if there is a wall in front of the enemy
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f),
                direction, wallCheckDistance, LayerMask.GetMask(Utils.EnumToLayerName(GameLayer.Ground)));

            // Check if a wall was found
            if (hit.collider != null)
            {
                // Check if we are not already jumping and if not jump
                if (_rigidbody.velocity.y < 0.1f)
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            }
            else
            {
                MoveTowardsPatrolPoint();
            }
        }

        _animator.SetFloat(Speed, Mathf.Abs(_rigidbody.velocityX));
    }

    private void MoveTowardsPatrolPoint()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints.Points[_currentPoint].position.x) > 0.2f)
        {
            if (transform.position.x < patrolPoints.Points[_currentPoint].position.x)
            {
                _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocityY);
                _spriteRenderer.flipX = true;
            }
            else
            {
                _rigidbody.velocity = new Vector2(-moveSpeed, _rigidbody.velocityY);
                _spriteRenderer.flipX = false;
            }
        }
        else
        {
            WaitAtPatrolPoint();
        }
    }

    private void WaitAtPatrolPoint()
    {
        _rigidbody.velocity = new Vector2(0.0f, _rigidbody.velocityY);
        _waitCounter -= Time.deltaTime;

        if (!(_waitCounter <= 0.0f))
            return;

        _waitCounter = waitAtPoints;

        // Check if we are traversing the path in reverse
        if (_isInReverse)
        {
            // If going in reverse, decrement the current patrol point
            _currentPoint--;

            if (_currentPoint >= 0)
                return;

            // If re reached the first patrol point, we no longer want to go in reverse
            _isInReverse = false;
            _currentPoint++;
        }
        else
        {
            // If going forward, increment the current patrol point
            _currentPoint++;
            
            if (_currentPoint < patrolPoints.Points.Count)
                return;

            // Check if we want to traverse the path in reverse
            if (reverse)
            {
                _isInReverse = true;
                _currentPoint--;
            }
            else
            {
                // If not prevent the enemy from moving
                _canMove = false;
            }
        }
    }
}