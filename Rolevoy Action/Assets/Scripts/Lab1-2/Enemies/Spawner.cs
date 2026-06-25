using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Spawner : MonoBehaviour
{
    public event Action<EnemyView> OnEnemySpawned;

    [Header("Enemy Types")]
    [SerializeField] private List<GameObject> enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private int totalEnemies = 5;   
    [SerializeField] private float spawnRadius = 20f; 
    [SerializeField] private float minDistanceBetween = 3f;

    [Header("Alternative Area (rect)")]
    [SerializeField] private bool useAreaBounds = false; 
    [SerializeField] private Vector3 areaMin = new Vector3(-30, 0, -30);
    [SerializeField] private Vector3 areaMax = new Vector3(30, 0, 30);

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogError("Spawner: no enemy prefabs assigned!");
            return;
        }

        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject prefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
            Vector3 spawnPos = GetValidSpawnPosition(usedPositions);

            if (spawnPos != Vector3.zero)
            {
                GameObject enemyObject = Instantiate(prefab, spawnPos, Quaternion.identity);
                usedPositions.Add(spawnPos);
                EnemyView enemyView = enemyObject.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    OnEnemySpawned?.Invoke(enemyView);
                }
            }
            else
            {
                Debug.LogWarning($"Spawner: failed to find position for enemy {i}");
            }
        }
    }

    private Vector3 GetValidSpawnPosition(List<Vector3> existingPositions)
    {
        int attempts = 30;
        for (int i = 0; i < attempts; i++)
        {
            Vector3 candidate;
            if (useAreaBounds)
            {
                float x = UnityEngine.Random.Range(areaMin.x, areaMax.x);
                float z = UnityEngine.Random.Range(areaMin.z, areaMax.z);
                candidate = new Vector3(x, 0, z);
            }
            else
            {
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
                candidate = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            }

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                Vector3 finalPos = hit.position;

                bool tooClose = false;
                foreach (var pos in existingPositions)
                {
                    if (Vector3.Distance(finalPos, pos) < minDistanceBetween)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                    return finalPos;
            }
        }
        return Vector3.zero;
    }

}