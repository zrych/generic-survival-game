using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Reference to the player transform
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows
    public Vector3 offset;         // Camera offset from player

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
