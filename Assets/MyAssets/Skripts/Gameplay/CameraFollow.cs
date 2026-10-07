using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRb;

    [SerializeField] private float ySmoothSpeed = 8f;
    [SerializeField] private float startSetY = 6f;

    [SerializeField] private float xSmoothSpeed = 1.7f;
    [SerializeField] private float xCameraZone = 1.8f;
    [SerializeField] private float xLookAhead = 3.2f;

    private float cameraStartY;
    private float targetX;
    private float xVelocity;

    private bool targetXLocked;
    private float lastDirection;
    private PlayerController playerController;

    void Start()
    {
        cameraStartY = player.position.y + startSetY;
        targetX = transform.position.x;

        playerController = player.GetComponent<PlayerController>();
        lastDirection = playerController.MoveDirection;

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
            if (targetXLocked)
            {
                targetX = transform.position.x + (targetX - transform.position.x) * 0.2f;
            }

            targetXLocked = false;
        }

        else
        {
            if (playerController.MoveDirection != lastDirection)
            {
                lastDirection = playerController.MoveDirection;
                targetXLocked = false;
            }

            if (!targetXLocked &&
                playerController.MoveDirection > 0 &&
                player.position.x > transform.position.x + xCameraZone)
            {
                targetX = player.position.x + xLookAhead;
                targetXLocked = true;
            }

            if (!targetXLocked &&
                playerController.MoveDirection < 0 &&
                player.position.x < transform.position.x - xCameraZone)
            {
                targetX = player.position.x - xLookAhead;
                targetXLocked = true;
            }
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
    }

}
