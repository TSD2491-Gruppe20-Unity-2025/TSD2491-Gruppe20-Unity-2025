using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Enemy Prefabs

    public GameObject basicEnemyPrefab;
    public GameObject bossEnemyPrefab;

    //-----------------------------------------------------------------------------//
    // Spawn Timers

    public float spawnInterval = 2f;

    private float timer = 0f;
    private float bossTimer = 0f;
    private bool bossSpawned = false;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    void Update()
    {
        timer += Time.deltaTime;
        bossTimer += Time.deltaTime;

        // Spawn boss after 15 seconds
        if (bossTimer >= 15f && !bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
            return;
        }

        // Spawn basic enemies at intervals until boss appears
        if (!bossSpawned && timer >= spawnInterval)
        {
            SpawnBasicEnemy();
            timer = 0f;
        }
    }

    //-----------------------------------------------------------------------------//
    // Enemy Spawning Methods

    void SpawnBasicEnemy()
    {
        Vector2 camTop = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0.1f, 0.9f), 1));
        Vector3 spawnPos = new Vector3(camTop.x, camTop.y + 1f, 0);
        Instantiate(basicEnemyPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnBoss()
    {
        Vector2 camTop = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));
        Vector3 spawnPos = new Vector3(camTop.x, camTop.y + 2f, 0);
        Instantiate(bossEnemyPrefab, spawnPos, Quaternion.identity);
    }
}