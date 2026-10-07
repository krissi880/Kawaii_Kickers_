using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float fallParallax = 0.2f;
    [SerializeField] private float fallStartThreshold = 2.5f;
    [SerializeField] private float fallDistanceForMaxOffset = 2f;

    [SerializeField] private float parallaxAmount = 0.1f;
    [SerializeField] private float smoothSpeed = 3f;

    [SerializeField] private float minOffsetX = -1f;
    [SerializeField] private float maxOffsetX = 1f;

    [SerializeField] private float minOffsetY = -2f;
    [SerializeField] private float maxOffsetY = 0.5f;

    [SerializeField] private float startParallaxY = 4f;

    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;
    private Vector3 lastCameraPosition;

    private float fallStartY;
    private bool isFalling;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        targetLocalPosition = startLocalPosition;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        targetLocalPosition.x -= cameraDelta.x * parallaxAmount;

        float targetY = startLocalPosition.y;

        if (cameraTransform.position.y > startParallaxY)
        {
            float cameraYDistance = cameraTransform.position.y - startParallaxY;

            targetY -= cameraYDistance * parallaxAmount;
        }

        float fallOffset = 0f;


        if (playerRb.linearVelocity.y < 0f)
        {
            if (!isFalling)
            {
                isFalling = true;
                fallStartY = playerRb.position.y;
            }

            float fallDistance = fallStartY - playerRb.position.y;

            

            if (fallDistance > fallStartThreshold)
            {
                fallOffset = fallParallax;
            }
        }
        else
        {
            isFalling = false;
        }

        float clampedY = Mathf.Clamp(
            targetY,
            startLocalPosition.y + minOffsetY,
            startLocalPosition.y + maxOffsetY
        );

        targetLocalPosition.y = clampedY - fallOffset;

        targetLocalPosition.x = Mathf.Clamp(
            targetLocalPosition.x,
            startLocalPosition.x + minOffsetX,
            startLocalPosition.x + maxOffsetX
        );


        float smooth = 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetLocalPosition,
            smooth
        );

        lastCameraPosition = cameraTransform.position;
    }
}