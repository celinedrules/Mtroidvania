using Cinemachine;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float camSpeed;

    private CinemachineVirtualCamera _cam;
    private float _startTime;
    private float _journeyLength;
    private Vector3 _cameraOrigin;
    private Vector3 _cameraDestination;

    private void Start()
    {
        _cam = FindAnyObjectByType<CinemachineVirtualCamera>(); 
        _cam.Follow = null;

        _cameraOrigin = new Vector3(pointA.position.x, pointA.position.y, _cam.transform.position.z);
        _cameraDestination = new Vector3(pointB.position.x, pointB.position.y, _cam.transform.position.z);
        
        _cam.transform.position = _cameraOrigin;
        
        _journeyLength = Vector3.Distance(_cameraOrigin, _cameraDestination);
    }

    private void Update()
    {
        float distCovered = (Time.time - _startTime) * camSpeed;
        float fractionOfJourney = distCovered / _journeyLength;
        
        _cam.transform.position = Vector3.Lerp(_cameraOrigin, _cameraDestination, fractionOfJourney);
    }   
}