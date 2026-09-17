using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float ySmoothSpeed = 8f;
    [SerializeField] private float xSmoothSpeed = 4f;
    [SerializeField] private float xDeadZone = 2f;
    [SerializeField] private float startSetY = 2f;

    private float cameraStartY;

    void Start()
    {
        cameraStartY = player.position.y + startSetY;
    }

    void LateUpdate()
    {
        float targetY = player.position.y;

        float newY = Mathf.Lerp(
            transform.position.y,
            targetY,
            8f * Time.deltaTime
    );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }


    void Update()
    {
        
    }
}
