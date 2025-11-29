using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Transform player;            // Reference to player transform
    public float maxRange = 3f;         // Max distance crosshair can be from player
    public SpriteRenderer playerSprite;

    void Update()
    {

            // Get mouse position in world space
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;  // Because it’s 2D


        // Calculate direction from player to mouse
        Vector3 direction = mouseWorldPos - player.position;
        float distance = direction.magnitude;

        // Clamp the distance to maxRange
        if (distance > maxRange)
        {
            direction = direction.normalized * maxRange;
        }

        // Set crosshair position relative to player
        transform.position = player.position + direction;

        if (direction.x < 0f)
        {
            playerSprite.flipX = true;
        } else
        {
            playerSprite.flipX = false;
        }
    }
}
