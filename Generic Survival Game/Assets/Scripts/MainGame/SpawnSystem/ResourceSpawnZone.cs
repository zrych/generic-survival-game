using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceSpawnOption
{
    public GameObject prefab;
    [Range(1, 100)]
    public int weight = 10;   // Higher = more common
}

public class ResourceSpawnZone : MonoBehaviour
{

    [SerializeField] private Transform worldSpawnPoint;

    // Tier radii
    [SerializeField] private float innerTierRadius = 20f;
    [SerializeField] private float midTierRadius = 40f;

    // Tier prefabs
    [SerializeField] private GameObject[] innerTierPrefabs;  // e.g., trees + boulders
    [SerializeField] private GameObject[] midTierPrefabs;    // e.g., coal
    [SerializeField] private GameObject[] outerTierPrefabs;  // e.g., iron

    [Header("Spawning")]
    [SerializeField] private ResourceSpawnOption[] resourceOptions;
    [SerializeField] private Vector2 zoneSize = new Vector2(10f, 10f);
    [SerializeField] private int maxCount = 10;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] private float minPlayerDistance = 5f;

    [Header("Spacing")]
    [SerializeField] private float minDistanceBetweenNodes = 1.2f;
    [SerializeField] private int maxSpawnAttempts = 30;

    private List<GameObject> activeNodes = new List<GameObject>();
    private Queue<float> respawnQueue = new Queue<float>();

    private Transform player;

    [SerializeField] private LayerMask blockingMask;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateInitialNodes();
    }

    public void GenerateInitialNodes()
    {
        for (int i = 0; i < maxCount; i++)
        {
            SpawnNode();
        }
    }

    public void QueueRespawn()
    {
        respawnQueue.Enqueue(Time.time + respawnTime);
    }

    private void Update()
    {
        HandleRespawnQueue();
    }

    private GameObject GetTieredPrefab(Vector2 spawnPos)
    {
        float dist = Vector2.Distance(worldSpawnPoint.position, spawnPos);

        if (dist <= innerTierRadius)
        {
            return innerTierPrefabs[Random.Range(0, innerTierPrefabs.Length)];
        }
        else if (dist <= midTierRadius)
        {
            return midTierPrefabs[Random.Range(0, midTierPrefabs.Length)];
        }
        else
        {
            return outerTierPrefabs[Random.Range(0, outerTierPrefabs.Length)];
        }
    }

    private GameObject GetRandomPrefab()
    {
        int totalWeight = 0;

        foreach (var option in resourceOptions)
            totalWeight += option.weight;

        int randomValue = Random.Range(0, totalWeight);

        int current = 0;

        foreach (var option in resourceOptions)
        {
            current += option.weight;
            if (randomValue < current)
                return option.prefab;
        }

        return resourceOptions[0].prefab; // fallback
    }

    private void HandleRespawnQueue()
    {
        if (respawnQueue.Count == 0) return;

        if (activeNodes.Count >= maxCount) return;

        float nextTime = respawnQueue.Peek();
        if (Time.time < nextTime) return;

        if (Vector2.Distance(player.position, transform.position) < minPlayerDistance)
            return;

        Debug.Log("Node has been respawned");
        respawnQueue.Dequeue();
        SpawnNode();
    }

    private void SpawnNode()
    {
        Vector2 spawnPos;
        bool foundPos = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float x = Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f);
            float y = Random.Range(-zoneSize.y / 2f, zoneSize.y / 2f);

            spawnPos = (Vector2)transform.position + new Vector2(x, y);

            if (!Physics2D.OverlapCircle(spawnPos, minDistanceBetweenNodes, blockingMask))
            {
                //GameObject prefab = GetTieredPrefab(spawnPos); //By Tier Zones
                GameObject prefab = GetRandomPrefab(); //randomized
                GameObject node = Instantiate(prefab, spawnPos, Quaternion.identity);
                activeNodes.Add(node);

                // Link the node back to this zone
                ResourceNodeTracker tracker = node.AddComponent<ResourceNodeTracker>();
                tracker.zone = this;

                foundPos = true;
                break;
            }
        }
    }

    public void RemoveNode(GameObject node)
    {
        Debug.Log("Node removed!");
        activeNodes.Remove(node);
        QueueRespawn();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}