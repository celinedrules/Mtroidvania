using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    private Vector3 _offset;
    
    void Start()
    {
        _offset.z = transform.position.z;
    }

    void Update()
    {
        transform.position = PlayerHealth.Instance.transform.position + _offset;
    }
}
