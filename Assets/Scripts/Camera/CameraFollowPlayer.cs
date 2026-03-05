using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);//adjust y offset if needed

    [Header("Player Information")]
    [SerializeField] private GameObject player;

    [Header("World Bounds (GameObjects with BoxCollider2D)")]
    [SerializeField] private GameObject LeftWorldBounds;
    [SerializeField]private GameObject RightWorldBounds;
    [SerializeField] private GameObject TopWorldBounds;
    [SerializeField] private GameObject BottomWorldBounds;

    private Camera cam;
    private float cameraHalfWidth;
    private float cameraHalfHeight;
    private float minX, maxX, minY, maxY; //camer center limits

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        cam = GetComponent<Camera>();
        CalculateCameraBounds();
    }

    private void CalculateCameraBounds()
    {
        //camera's half dimensions
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * cam.aspect;

        //default to no limits
        minX = float.NegativeInfinity;
        maxX = float.PositiveInfinity;
        minY = float.NegativeInfinity;
        maxY = float.PositiveInfinity;

        //check if all four bounds are assigned and have BoxCollider2D
        if(LeftWorldBounds != null && RightWorldBounds != null && TopWorldBounds != null && BottomWorldBounds != null)
        {
            BoxCollider2D leftCol = LeftWorldBounds.GetComponent<BoxCollider2D>();
            BoxCollider2D rightCol = RightWorldBounds.GetComponent<BoxCollider2D>();
            BoxCollider2D topCol = TopWorldBounds.GetComponent<BoxCollider2D>();
            BoxCollider2D bottomCol = BottomWorldBounds.GetComponent<BoxCollider2D>();

            if (leftCol != null && rightCol != null && topCol != null && bottomCol != null)
            {
                // Inner edges of the colliders (the world boundary lines)
                float leftEdge = leftCol.transform.position.x + leftCol.bounds.extents.x;
                float rightEdge = rightCol.transform.position.x - rightCol.bounds.extents.x;
                float bottomEdge = bottomCol.transform.position.y + bottomCol.bounds.extents.y;
                float topEdge = topCol.transform.position.y - topCol.bounds.extents.y;

                // Camera center must stay at least half width/height away from edges
                minX = leftEdge + cameraHalfWidth;
                maxX = rightEdge - cameraHalfWidth;
                minY = bottomEdge + cameraHalfHeight;
                maxY = topEdge - cameraHalfHeight;
            }
            else
            {
                Debug.LogWarning("One or more world bounds are missing a BoxCollider2D");
            }
        }
        else
        {
            Debug.LogWarning("Not all world bounds are assigned. Camera will not be clamped vertically (or at all)");
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        //target position = player + offset
        Vector3 targetPosition = player.transform.position + offset;

        //apply bounds
        Vector3 boundedPosition = GetBoundedPosition(targetPosition);

        //smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,boundedPosition,cameraSpeed * Time.deltaTime);

        //update camera position
        transform.position = smoothedPosition;
    }

    private Vector3 GetBoundedPosition(Vector3 targetPos)
    {
        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
        return new Vector3(clampedX, clampedY, transform.position.z);
    }
}
