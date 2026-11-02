using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(20f, 20f);
    [SerializeField] private float spawnHeight = 10f;

    private readonly List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool respawnScheduled;

    private void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        if (spawnedEnemies.Count == 0 && !respawnScheduled)
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        respawnScheduled = true;
        yield return new WaitForSeconds(respawnDelay);
        respawnScheduled = false;
        SpawnWave();
    }

    private void SpawnWave()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner has no enemy prefab assigned.", this);
            return;
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f),
            0f,
            Random.Range(-spawnAreaSize.y * 0.5f, spawnAreaSize.y * 0.5f));

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, spawnHeight, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return randomPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
    }
}
