using Cinemachine;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float camSpeed;
    [SerializeField] private int threashold1;
    [SerializeField] private int threashold2;
    [SerializeField] private float activeTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float inactiveTime;
    [SerializeField] private PatrolPoints patrolPoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform boss;

    private CinemachineVirtualCamera _cam;
    private float _startTime;
    private float _journeyLength;
    private Vector3 _cameraOrigin;
    private Vector3 _cameraDestination;
    private Transform _followTarget;

    private Animator _animator;
    private float _activeCounter;
    private float _fadeCounter;
    private float _inactiveCounter;
    private Transform _targetPoint;
    private static readonly int Vanish = Animator.StringToHash("Vanish");

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _cam = FindAnyObjectByType<CinemachineVirtualCamera>();
        _followTarget = _cam.Follow;
        _cam.Follow = null;

        _cameraOrigin = new Vector3(pointA.position.x, pointA.position.y, _cam.transform.position.z);
        _cameraDestination = new Vector3(pointB.position.x, pointB.position.y, _cam.transform.position.z);
        
        _cam.transform.position = _cameraOrigin;
        
        _journeyLength = Vector3.Distance(_cameraOrigin, _cameraDestination);

        _activeCounter = activeTime;
    }

    private void Update()
    {
        float distCovered = (Time.time - _startTime) * camSpeed;
        float fractionOfJourney = distCovered / _journeyLength;
        
        _cam.transform.position = Vector3.Lerp(_cameraOrigin, _cameraDestination, fractionOfJourney);

        // Phase 1
        if (BossHealth.Instance.CurrentHealth > threashold1)
        {
            if (_activeCounter > 0)
            {
                _activeCounter -= Time.deltaTime;

                if (_activeCounter <= 0)
                {
                    _fadeCounter = fadeOutTime;
                    _animator.SetTrigger(Vanish);
                }
            }
            else if (_fadeCounter > 0)
            {
                _fadeCounter -= Time.deltaTime;
                
                if(!(_fadeCounter <= 0))
                    return;
                
                boss.gameObject.SetActive(false);
                _inactiveCounter = inactiveTime;
            }
            else if (_inactiveCounter > 0)
            {
                _inactiveCounter -= Time.deltaTime;
                
                if(!(_inactiveCounter <= 0))
                    return;

                boss.position = patrolPoints.Points[Random.Range(0, patrolPoints.Points.Count)].position;
                boss.gameObject.SetActive(true);

                _activeCounter = activeTime;
            }
        }
        // Phase 2
        else
        {
            
        }
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
    }
}