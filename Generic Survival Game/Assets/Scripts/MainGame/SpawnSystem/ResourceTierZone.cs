using UnityEngine;

public class ResourceTierZone : MonoBehaviour
{
    public float minRadius;
    public float maxRadius;

    [Header("Allowed Resources")]
    public ResourceSpawnOption[] allowedResources;
    public Color debugColor = Color.green;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(debugColor.r, debugColor.g, debugColor.b, 0.2f);
        Gizmos.DrawWireSphere(Vector3.zero, maxRadius);

        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.15f);
        Gizmos.DrawWireSphere(Vector3.zero, minRadius);
    }
    public bool IsInside(Vector2 worldPoint)
    {
        float distance = Vector2.Distance(Vector2.zero, worldPoint); // Assuming spawn = (0,0)
        return distance >= minRadius && distance <= maxRadius;
    }
}
