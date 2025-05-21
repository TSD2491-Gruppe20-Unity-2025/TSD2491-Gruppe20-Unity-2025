using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject basicEnemyPrefab;
    public GameObject bossEnemyPrefab;

    public float spawnInterval = 2f;

    private float timer = 0f;
    private float bossTimer = 0f;
    private bool bossSpawned = false;

    void Update()
    {
        timer += Time.deltaTime;
        bossTimer += Time.deltaTime;

        if (bossTimer >= 60f && !bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
            return;
        }

        if (!bossSpawned && timer >= spawnInterval)
        {
            SpawnBasicEnemy();
            timer = 0f;
        }
    }

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
