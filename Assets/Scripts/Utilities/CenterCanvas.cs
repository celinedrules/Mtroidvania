using UnityEngine;

public class CenterCanvas : MonoBehaviour
{
    public Camera uiCamera; // Assign the camera you're using to render the UI
    public float distanceFromCamera = 5.0f; // Distance in front of the camera to position the Canvas

    private void Start()
    {
        uiCamera = Camera.main;
    }

    void Update()
    {
        // Position the Canvas in front of the camera
        transform.position = uiCamera.transform.position + uiCamera.transform.forward * distanceFromCamera;

        // Optionally, ensure the Canvas always faces the camera directly
        transform.LookAt(transform.position + uiCamera.transform.rotation * Vector3.forward,
            uiCamera.transform.rotation * Vector3.up);
    }
}