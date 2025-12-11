using UnityEngine;

public class ResourceZoneManager : MonoBehaviour
{
    public static ResourceZoneManager Instance;

    public ResourceTierZone[] zones;

    private void Awake()
    {
        Instance = this;
    }

    public ResourceTierZone GetZoneForPosition(Vector2 pos)
    {
        foreach (var zone in zones)
        {
            if (zone.IsInside(pos))
                return zone;
        }
        return null;
    }
}
