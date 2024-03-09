using System.Linq;
using Cinemachine;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BossBattle : MonoBehaviour, IJsonSavable
{
    [SerializeField] private BossActivator activator;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float camSpeed;
    [SerializeField] private int threshold1;
    [SerializeField] private int threshold2;
    [SerializeField] private float activeTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float inactiveTime;
    [SerializeField] private PatrolPoints patrolPoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform boss;
    [SerializeField] private float timeBetweenShots1;
    [SerializeField] private float timeBetweenShots2;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject winObjects;

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
    private float _shotCounter;
    private bool _battleEnded;
    
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
        _shotCounter = timeBetweenShots1;
        
        AudioManager.Instance.PlayAudio(AudioType.BossBattle);
    }

    private void Update()
    {
        float distCovered = (Time.time - _startTime) * camSpeed;
        float fractionOfJourney = distCovered / _journeyLength;
        
        _cam.transform.position = Vector3.Lerp(_cameraOrigin, _cameraDestination, fractionOfJourney);

        if (!_battleEnded)
        {
            // Phase 1
            if (BossHealth.Instance.CurrentHealth > threshold1)
            {
                if (_activeCounter > 0)
                {
                    _activeCounter -= Time.deltaTime;

                    if (_activeCounter <= 0)
                    {
                        _fadeCounter = fadeOutTime;
                        _animator.SetTrigger(Vanish);
                    }

                    _shotCounter -= Time.deltaTime;
                    
                    if(!(_shotCounter <= 0))
                        return;

                    _shotCounter = timeBetweenShots1;
                    Instantiate(bullet, shotPoint.position, Quaternion.identity);
                }
                else if (_fadeCounter > 0)
                {
                    _fadeCounter -= Time.deltaTime;

                    if (!(_fadeCounter <= 0))
                        return;

                    boss.gameObject.SetActive(false);
                    _inactiveCounter = inactiveTime;
                }
                else if (_inactiveCounter > 0)
                {
                    _inactiveCounter -= Time.deltaTime;

                    if (!(_inactiveCounter <= 0))
                        return;

                    boss.position = patrolPoints.Points[Random.Range(0, patrolPoints.Points.Count)].position;
                    boss.gameObject.SetActive(true);

                    _activeCounter = activeTime;
                    _shotCounter = timeBetweenShots1;
                }
            }
            // Phase 2
            else
            {
                if (_targetPoint == null)
                {
                    _targetPoint = boss;
                    _fadeCounter = fadeOutTime;
                    _animator.SetTrigger(Vanish);
                }
                else
                {
                    if (Vector3.Distance(boss.position, _targetPoint.position) > 0.2f)
                    {
                        boss.position =
                            Vector3.MoveTowards(boss.position, _targetPoint.position, moveSpeed * Time.deltaTime);

                        if (Vector3.Distance(boss.position, _targetPoint.position) <= 0.2f)
                        {
                            _fadeCounter = fadeOutTime;
                            _animator.SetTrigger(Vanish);
                        }

                        _shotCounter -= Time.deltaTime;
                        
                        if(!(_shotCounter <= 0))
                            return;

                        _shotCounter = BossHealth.Instance.CurrentHealth > threshold2
                            ? timeBetweenShots1
                            : timeBetweenShots2;

                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }
                    else if (_fadeCounter > 0)
                    {
                        _fadeCounter -= Time.deltaTime;

                        if (!(_fadeCounter <= 0))
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
                        _targetPoint = patrolPoints.Points[Random.Range(0, patrolPoints.Points.Count)];

                        while (_targetPoint.position == boss.position)
                            _targetPoint = patrolPoints.Points[Random.Range(0, patrolPoints.Points.Count)];

                        boss.gameObject.SetActive(true);
                        _shotCounter = BossHealth.Instance.CurrentHealth > threshold2
                            ? timeBetweenShots1
                            : timeBetweenShots2;
                    }
                }
            }
        }
        else
        {
            _fadeCounter -= Time.deltaTime;
            
            if(!(_fadeCounter < 0))
                return;

            if (winObjects != null)
            {
                winObjects.SetActive(true);
                winObjects.transform.SetParent(null);
            }

            _cam.Follow = _followTarget;
            //gameObject.SetActive(false);
        }
    }

    public void EndBattle()
    {
        _battleEnded = true;
        _fadeCounter = fadeOutTime;
        _animator.SetTrigger(Vanish);
        boss.GetComponent<Collider2D>().enabled = false;

        BossBullet[] bullets = FindObjectsByType<BossBullet>(FindObjectsSortMode.None);

        foreach (BossBullet bossBullet in bullets)
            Destroy(bossBullet.gameObject);
        
        AudioManager.Instance.PlayAudio(AudioType.LevelMusic);
        
        Destroy(boss.gameObject);
    }

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(GetComponent<BossHealth>().CurrentHealth);
    }

    public void RestoreFromJToken(JToken state)
    {
        int bossHealth;

        // Check if 'state' is an object and contains 'BossBattle'
        if (state.Type == JTokenType.Object && state["BossBattle"] != null)
            bossHealth = state["BossBattle"].ToObject<int>();
        else // Assume 'state' is a direct integer value
            bossHealth = state.ToObject<int>();

        GetComponent<BossHealth>().CurrentHealth = bossHealth;

        if (bossHealth <= 0)
        {
            activator.gameObject.SetActive(false);
            DestroyImmediate(gameObject);
        }
    }

}