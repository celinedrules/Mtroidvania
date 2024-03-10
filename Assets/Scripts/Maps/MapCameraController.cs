using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public Transform playerMarker; // Assign in the inspector
    public PolygonCollider2D minimapBounds; // Assign the PolygonCollider2D representing your minimap bounds
    public float edgeThreshold = 1.0f; // Distance within which to consider the player marker close to an edge
    public LayerMask levelBoundaryLayer; // Layer containing the level boundaries

    private Vector3 _offset;
    private RaycastHit2D _hitUp;
    private RaycastHit2D _hitDown;
    private RaycastHit2D _hitLeft;
    private RaycastHit2D _hitRight;

    void Start()
    {
        if (!playerMarker || !minimapBounds)
        {
            Debug.LogWarning("Dependencies are not properly set on MapCameraController.");
            enabled = false; // Disable script if dependencies are not set
            return;
        }

        _offset = transform.position - playerMarker.position;
    }

    private void Update()
    {
        CheckForMinimapEdgeProximity();
    }

    void LateUpdate()
    {
        if (playerMarker == null)
            return;

        // Start with the current camera position
        Vector3 newPosition = transform.position;

        // Calculate the target position based on the player marker and offset
        Vector3 targetPosition = playerMarker.position + _offset;

        // Check each direction. If there is no collider hit in that direction,
        // update the camera position component for that direction to follow the player marker.
        // This approach allows the camera to follow the player marker in directions
        // where there is no nearby edge detected.

        // Left and Right
        if (!_hitLeft.collider && !_hitRight.collider)
            newPosition.x = targetPosition.x;
        else if (!_hitLeft.collider) // Close to the right edge but not the left
            newPosition.x = Mathf.Min(newPosition.x, targetPosition.x);
        else if (!_hitRight.collider) // Close to the left edge but not the right
            newPosition.x = Mathf.Max(newPosition.x, targetPosition.x);

        // Up and Down
        if (!_hitUp.collider && !_hitDown.collider)
            newPosition.y = targetPosition.y;
        else if (!_hitUp.collider) // Close to the lower edge but not the upper
            newPosition.y = Mathf.Min(newPosition.y, targetPosition.y);
        else if (!_hitDown.collider) // Close to the upper edge but not the lower
            newPosition.y = Mathf.Max(newPosition.y, targetPosition.y);

        // Finally, update the camera position
        transform.position = newPosition;
    }

    void CheckForMinimapEdgeProximity()
    {
        // Cast rays in the up, down, left, and right directions
        Vector3 playerMarkerPosition = playerMarker.position;
        
        _hitUp = Physics2D.Raycast(playerMarkerPosition, Vector2.up, edgeThreshold, levelBoundaryLayer);
        _hitDown = Physics2D.Raycast(playerMarkerPosition, Vector2.down, edgeThreshold, levelBoundaryLayer);
        _hitLeft = Physics2D.Raycast(playerMarkerPosition, Vector2.left, edgeThreshold, levelBoundaryLayer);
        _hitRight = Physics2D.Raycast(playerMarkerPosition, Vector2.right, edgeThreshold, levelBoundaryLayer);

        if (GameManager.Instance.Debug)
        {
            // Optionally, visualize the rays in the Scene view for debugging
            Debug.DrawRay(playerMarkerPosition, Vector2.up * edgeThreshold, Color.red);
            Debug.DrawRay(playerMarkerPosition, Vector2.down * edgeThreshold, Color.green);
            Debug.DrawRay(playerMarkerPosition, Vector2.left * edgeThreshold, Color.blue);
            Debug.DrawRay(playerMarkerPosition, Vector2.right * edgeThreshold, Color.yellow);

            // Check if any ray hit the level boundary
            if (_hitUp.collider != null)
                Debug.Log("Close to the upper edge of the minimap");

            if (_hitDown.collider != null)
                Debug.Log("Close to the lower edge of the minimap");

            if (_hitLeft.collider != null)
                Debug.Log("Close to the left edge of the minimap");

            if (_hitRight.collider != null)
                Debug.Log("Close to the right edge of the minimap");
        }
    }
}