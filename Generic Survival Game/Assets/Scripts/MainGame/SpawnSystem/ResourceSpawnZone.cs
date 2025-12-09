using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawnZone : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private GameObject resourceNode;
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

            if (Physics2D.OverlapCircle(spawnPos, minDistanceBetweenNodes) == null)
            {
                GameObject node = Instantiate(resourceNode, spawnPos, Quaternion.identity);
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