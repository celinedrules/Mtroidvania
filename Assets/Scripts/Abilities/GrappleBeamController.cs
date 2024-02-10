using UnityEngine;

public class GrappleBeamController : Weapon
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float minGrappleDistance;
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private LineRenderer beam;
    [SerializeField] private float lineSegmentLength = 0.5f;
    [SerializeField] private int numberOfSegments = 10;
    [SerializeField] private DistanceJoint2D joint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LayerMask layersToIgnore;

    private Camera _mainCamera;
    private Vector3 _grapplePoint;
    private Vector3 _shotPointPosition;
    
    private Transform PlayerTransform => player.transform;
    private Vector3 PlayerPosition => player.transform.position;

    protected override void Start()
    {
        _mainCamera = Camera.main;
        joint.enabled = false;
        beam.enabled = false;
        beam.positionCount = numberOfSegments;
    }

    public override void Fire(Vector3 position, bool flipX = false)
    {
        _shotPointPosition = position;
        
        if (_mainCamera != null)
        {
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            bool isBehindPlayer = PlayerTransform.localScale.x > 0
                ? mouseWorldPosition.x < PlayerPosition.x
                : mouseWorldPosition.x > PlayerPosition.x;

            if (!isBehindPlayer)
                Grapple();
        }
    }
    
    public void Cancel()
    {
        joint.enabled = false;
        beam.enabled = false;
    }

    private void Update()
    {
        beam.SetPosition(1, _shotPointPosition);
        AnimateRope();
    }

    private void AnimateRope()
    {
        Vector3 startPoint = _shotPointPosition;

        for (int i = 0; i < numberOfSegments; i++)
        {
            float segmentRatio = (float)i / (numberOfSegments - 1);
            Vector3 segmentPoint = Vector3.Lerp(startPoint, _grapplePoint, segmentRatio);


            if (i > 0 && i < numberOfSegments - 1)
            {
                float noise = Mathf.PerlinNoise(Time.time * 5f, segmentRatio * 5f) * (2f - 1f);
                Vector3 noiseDirection = Quaternion.Euler(0, 0, 90) * (_grapplePoint - startPoint).normalized;
                float maxOffset = lineSegmentLength * 0.1f;
                segmentPoint += noiseDirection * (noise * Mathf.Min(maxOffset, (lineSegmentLength / numberOfSegments)));
            }

            beam.SetPosition(i, segmentPoint);
        }
    }

    private void Grapple()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        Vector3 directionToMouse = (mouseWorldPosition - _shotPointPosition).normalized;
        RaycastHit2D hit = Physics2D.Raycast(_shotPointPosition, directionToMouse, maxGrappleDistance, ~layersToIgnore);

        if (hit.collider != null)
        {
            // Check if the hit object is what you consider a valid grapple target
            if (((1 << hit.collider.gameObject.layer) & grappleLayer) != 0)
            {
                // The object hit is on the grappleLayer, handle grappling logic
                _grapplePoint = hit.point;
                _grapplePoint.z = 0;
                joint.connectedAnchor = _grapplePoint;
                joint.enabled = true;
                joint.distance = minGrappleDistance;
                beam.enabled = true;
            }
            else
            {
                // The object hit is not a valid grapple target but should block the rope
                _grapplePoint = hit.point;
                _grapplePoint.z = 0;
                joint.enabled = false; // Disable the joint as it's not a valid grapple target
                beam.enabled = true; // Still show the rope to indicate obstruction
            }
        }
        else
        {
            // No object was hit, optionally extend the rope to max distance for visual feedback
            Vector3 noHitPoint = _shotPointPosition + directionToMouse * maxGrappleDistance;
            _grapplePoint = noHitPoint;
            beam.enabled = true; // Show the rope extending to max distance
            joint.enabled = false; // Ensure the joint is disabled
        }

        // Always set the rope's positions for visualization
        beam.SetPosition(0, _shotPointPosition);
        beam.SetPosition(1, _grapplePoint);
    }
}