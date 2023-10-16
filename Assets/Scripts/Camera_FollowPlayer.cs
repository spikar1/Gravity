using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera_FollowPlayer : MonoBehaviour
{
    new Camera camera;
    Transform playerTransform;

    [SerializeField] int minX, maxX, minY, maxY;



    private void OnValidate()
    {
        if(playerTransform == null)
            playerTransform = FindAnyObjectByType<Player>().transform;
        if(camera == null)
            camera = GetComponent<Camera>();
    }

    public void InitCamera(int minX, int maxX, int minY, int maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;

        var newPosition = transform.position;
        if (transform.position.x < minX)
            newPosition.x = minX;
        if (transform.position.x > maxX)
            newPosition.x = maxX;
        if (transform.position.y < minY)
            newPosition.y = minY;
        if (transform.position.y > maxY)
            newPosition.y = maxY;

        transform.position = newPosition;
    }

    private void Awake()
    {
        if (playerTransform == null)
            playerTransform = FindAnyObjectByType<Player>().transform;
        if (camera == null)
            camera = GetComponent<Camera>();

        InitCamera(minX, maxX, minY, maxY);
    }

    private void LateUpdate()
    {
        var newPosition = transform.position;
        newPosition.z = -10;
        if(playerTransform.position.x > minX && playerTransform.position.x < maxX)
            newPosition.x = playerTransform.position.x;
        if(playerTransform.position.y > minY && playerTransform.position.y < maxY)
            newPosition.y = playerTransform.position.y;



        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        var center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, -10);

        Gizmos.DrawWireCube(center, new Vector3((camera.orthographicSize * 2 * 1.3f) + (maxX - minX), camera.orthographicSize * 2 + (maxY - minY)));
    }
}
