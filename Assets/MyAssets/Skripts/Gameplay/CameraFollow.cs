using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRb;

    [SerializeField] private float ySmoothSpeed = 8f;
    [SerializeField] private float startSetY = 6f;

    [SerializeField] private float xSmoothSpeed = 2f;
    [SerializeField] private float hitPointSwitchMargin = 0.75f;
    [SerializeField] private float hitPointSearchInterval = 0.15f;

    private float cameraStartY;
    private float targetX;
    private float xVelocity;
    private float nextHitPointSearchTime;

    private Transform currentHitPoint;

    private Vector3 shakeOffset;
    private float shakeDuration;
    private float shakeTimeRemaining;
    private float shakeMagnitude;

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeTimeRemaining = duration;
        shakeMagnitude = magnitude;
    }

    void Start()
    {
        cameraStartY = player.position.y + startSetY;
        targetX = transform.position.x;

        transform.position = new Vector3(
            transform.position.x,
            cameraStartY,
            transform.position.z
        );
    }

    void LateUpdate()
    {
        transform.position -= shakeOffset;
        shakeOffset = Vector3.zero;

        if (Time.time >= nextHitPointSearchTime)
        {
            FindHitPoint();
            nextHitPointSearchTime =
                Time.time + hitPointSearchInterval;
        }

        float newX = Mathf.SmoothDamp(
            transform.position.x,
            targetX,
            ref xVelocity,
            1f / xSmoothSpeed
        );

        float newY = transform.position.y;

        if (player.position.y > transform.position.y)
        {
            newY = Mathf.Lerp(
                transform.position.y,
                player.position.y,
                ySmoothSpeed * Time.deltaTime
            );
        }

        transform.position = new Vector3(
            newX,
            newY,
            transform.position.z
        );

        if (shakeTimeRemaining > 0f)
        {
            float strength =
                shakeMagnitude * (shakeTimeRemaining / shakeDuration);

            shakeOffset = UnityEngine.Random.insideUnitCircle * strength;
            transform.position += shakeOffset;

            shakeTimeRemaining -= Time.deltaTime;
        }
    }

    void FindHitPoint()
    {
        GameObject[] points =
            GameObject.FindGameObjectsWithTag("HitPoint");

        Transform bestPoint = null;
        float bestDistance = float.MaxValue;

        foreach (GameObject pointObject in points)
        {
            Transform point = pointObject.transform;

            if (point == currentHitPoint)
                continue;

            if (point.position.y <= player.position.y)
                continue;

            float distance = Vector2.Distance(
                player.position,
                point.position
            );

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestPoint = point;
            }
        }

        if (currentHitPoint == null)
        {
            if (bestPoint != null)
            {
                currentHitPoint = bestPoint;
                targetX = bestPoint.position.x;
            }

            return;
        }

        float currentDistance = Vector2.Distance(
            player.position,
            currentHitPoint.position
        );

        if (bestPoint != null &&
            bestDistance + hitPointSwitchMargin < currentDistance)
        {
            currentHitPoint = bestPoint;
            targetX = bestPoint.position.x;
        }
    }
}