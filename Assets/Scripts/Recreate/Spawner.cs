using UnityEngine;
using System.Collections.Generic;

public class EndlessRunnerMap2D : MonoBehaviour
{
    public GameObject[] prefabTiles; // Tile prefabs
    public GameObject[] enemyPrefabs; // Enemy prefabs
    public float tileHeight = 10f;   // Height of each tile
    public int tilesOnScreen = 5;    // Visible tile count
    public float scrollSpeed = 2f;   // Downward speed
    public float enemySpawnInterval = 2f; // Time between enemy spawns

    private List<GameObject> activeTiles = new List<GameObject>();
    private float nextSpawnY = 0f;
    private float enemySpawnTimer = 0f;

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile(i < 2 ? 0 : -1);
        }
    }

    void Update()
    {
        foreach (var tile in activeTiles)
        {
            tile.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        }

        if (activeTiles.Count > 0 && activeTiles[0].transform.position.y < -tileHeight)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
            SpawnTile();
        }

        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemySpawnTimer = 0f;
        }
    }

    void SpawnTile(int prefabIndex = -1)
    {
        if (prefabIndex == -1)
        {
            prefabIndex = Random.Range(0, prefabTiles.Length);
        }

        GameObject tile = Instantiate(prefabTiles[prefabIndex], new Vector3(0, nextSpawnY, 0), Quaternion.identity);
        tile.transform.SetParent(transform);
        activeTiles.Add(tile);
        nextSpawnY += tileHeight;
    }

    void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        float spawnX = Random.Range(-8f, 8f); // Adjust based on your game width
        Vector3 spawnPosition = new Vector3(spawnX, Camera.main.orthographicSize + 1, 0);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
        enemy.transform.SetParent(transform);
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * scrollSpeed * Random.Range(1f, 2f); // Add some variation
        }
    }
}
