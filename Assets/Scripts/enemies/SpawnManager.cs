using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; // The specific enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points for this enemy type
    public float spawnInterval = 5f; // Time between spawns

    private void Start()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager requires an enemyPrefab and at least one spawn point.");
            return;
        }
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Choose a random spawn point from the array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Spawn the enemy prefab at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Wait for the next spawn cycle
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
