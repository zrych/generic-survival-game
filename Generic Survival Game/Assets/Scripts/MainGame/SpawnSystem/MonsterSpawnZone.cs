using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private Vector2 zoneSize = new Vector2(10, 10);
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float minDistanceBetweenMonsters = 1.5f;
    [SerializeField] private float distanceFromPlayer = 3f;
    [Header("Monster Settings")]
    public GameObject zombiePrefab;
    public int maxMonsters = 5;
    public float spawnInterval = 3f;

    public List<GameObject> activeMonsters = new List<GameObject>();
    private bool canSpawn = false;
    private float spawnTimer;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MonsterSpawnManager.Instance.RegisterZone(this);
    }

    private void Update()
    {
        if (!canSpawn) return;

        // remove killed monsters
        activeMonsters.RemoveAll(m => m == null);

        if (activeMonsters.Count >= maxMonsters)
            return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            TrySpawn();
            spawnTimer = spawnInterval;
        }
    }

    public void EnableSpawning() => canSpawn = true;
    public void DisableSpawning() => canSpawn = false;

    private void TrySpawn()
    {
        for (int attempts = 0; attempts < 10; attempts++)
        {
            Vector2 pos = GetRandomPointInZone();

            // too close to player?
            if (Vector2.Distance(player.position, pos) < distanceFromPlayer)
                continue;

            // no space?
            if (Physics2D.OverlapCircle(pos, minDistanceBetweenMonsters, obstacleMask))
                continue;

            GameObject z = Instantiate(zombiePrefab, pos, Quaternion.identity);
            activeMonsters.Add(z);
            return;
        }
    }

    private Vector2 GetRandomPointInZone()
    {
        float x = Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f);
        float y = Random.Range(-zoneSize.y / 2f, zoneSize.y / 2f);
        return (Vector2)transform.position + new Vector2(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
