using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRb;

    [SerializeField] private float ySmoothSpeed = 8f;
    [SerializeField] private float startSetY = 4.4f;

    [SerializeField] private float xSmoothSpeed = 2f;
    [SerializeField] private float xCameraZone = 2f;
    [SerializeField] private float xLookAhead = 4f;

    private float cameraStartY;
    private float targetX;

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
        if (Mathf.Abs(playerRb.linearVelocity.x) < 0.01f)
        {
            targetX = transform.position.x;
        }
        else if (playerRb.linearVelocity.x > 0 &&
                 player.position.x > transform.position.x + xCameraZone)
        {
            targetX = player.position.x + xLookAhead;
        }
        else if (playerRb.linearVelocity.x < 0 &&
                 player.position.x < transform.position.x - xCameraZone)
        {
            targetX = player.position.x - xLookAhead;
        }

        float xLerpSpeed = 1f - Mathf.Exp(-xSmoothSpeed * Time.deltaTime);

        float newX = Mathf.Lerp(
            transform.position.x,
            targetX,
            xLerpSpeed
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
    }

}
