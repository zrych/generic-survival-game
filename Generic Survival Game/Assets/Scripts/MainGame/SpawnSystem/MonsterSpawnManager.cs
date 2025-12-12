using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance;

    public List<MonsterSpawnZone> zones = new List<MonsterSpawnZone>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RegisterZone(MonsterSpawnZone zone)
    {
        zones.Add(zone);
    }

    public void NightStarted()
    {
        foreach (var zone in zones)
            zone.EnableSpawning();
    }

    public void DayStarted()
    {
        foreach (var zone in zones)
        {
            zone.DisableSpawning();
            foreach (var z in zone.activeMonsters)
            {
                if (z != null)
                    Destroy(z);
            }
            zone.activeMonsters.Clear();
        }   
    }
}
