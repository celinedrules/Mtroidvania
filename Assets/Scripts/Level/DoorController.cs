using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int doorId;
    [SerializeField] private int destinationDoorId;

    [SerializeField, OnValueChanged("LockDoor")]
    private bool isLocked;

    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private float distanceToOpen = 7.5f;
    [SerializeField] private float moveDistance = 7.5f;
    [SerializeField] private float movePlayerSpeed = 10.0f;
    [SerializeField] private SceneReference sceneToLoad;
    [SerializeField] private Transform spawnPoint;

    private Animator _animator;
    private SpriteRenderer _doorSprite;
    private PlayerController _player;
    private bool _playerExiting;
    private static readonly int DoorOpen = Animator.StringToHash("DoorOpen");

    public int DoorId => doorId;
    public Transform SpawnPoint => spawnPoint;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _doorSprite = GetComponentInChildren<SpriteRenderer>();
        _player = PlayerHealth.Instance.GetComponent<PlayerController>();
    }

    private void Update()
    {
        _animator.SetBool(DoorOpen, Vector3.Distance(transform.position, _player.transform.position) < distanceToOpen);
        
        if(!_playerExiting)
            return;
        
        Vector3 position = SpawnPoint.transform.position;
        _player.transform.position = Vector3.MoveTowards(_player.transform.position,
            transform.localScale.x > 0.0f
                ? new Vector3(position.x + moveDistance, position.y, position.z)
                : new Vector3(position.x + -moveDistance, position.y, position.z),
            movePlayerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!_playerExiting)
            StartCoroutine(UseDoorCoroutine());
    }

    private IEnumerator UseDoorCoroutine()
    {
        PlayerState.Instance.DoorId = destinationDoorId;
        _player.CanMove = false;
        _playerExiting = true;
        _player.EnableAnimations(false);
        UIController.Instance.FadeInOut(true);

        yield return new WaitForSeconds(1.5f);

        _player.CanMove = true;
        _player.EnableAnimations(true);
        UIController.Instance.FadeInOut(false);
        
        // PlayerPrefs.SetString("ContinueLevel", sceneToLoad);
        // PlayerPrefs.SetFloat("PosX", spawnPoint.position.x);
        // PlayerPrefs.SetFloat("PosY", spawnPoint.position.y);
        
        //GameManager.Instance.SavePlayerPosition(sceneToLoad, spawnPoint.position);
        
        SceneManager.LoadScene(sceneToLoad);
    }

    private void LockDoor()
    {
        if (!Application.isPlaying)
            return;
        
        if (isLocked)
        {
            _animator.enabled = false;
            _doorSprite.sprite = lockedSprite;
        }
        else
        {
            _animator.enabled = true;
            _doorSprite.sprite = unlockedSprite;
        }
    }
}
