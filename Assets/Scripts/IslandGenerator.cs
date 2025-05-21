using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [Header("Island Settings")]
    public GameObject[] islandPrefabs; // Dra inn prefab-varianter i Inspector
    public float spawnInterval = 4f; //TODO: Gjør spawnintervallet tilfeldig, og la øyer som spawner for nært unngå kollisjon
    public float fallSpeed = 2f;
    public float spawnXMin = -8f;
    public float spawnXMax = 8f;
    public float spawnY = 10f;
    public float destroyY = -10f;
    public float tileSize = 1f; // 1 unit = 1 tile = 16 pixels
    public int minSlotX = -8;   // Leftmost tile slot index
    public int maxSlotX = 8;    // Rightmost tile slot index

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnIsland();
            timer = 0f;
        }
    }

    void SpawnIsland()
    {
        if (islandPrefabs.Length == 0) return;

        GameObject prefab = islandPrefabs[Random.Range(0, islandPrefabs.Length)];

        // Sørger for at øya spawner i kontekst av 16x16 pixel tiles
        int slotIndex = Random.Range(minSlotX, maxSlotX + 1);
        float snappedX = slotIndex * tileSize;

        Vector2 spawnPosition = new Vector2(snappedX, spawnY);
        GameObject island = Instantiate(prefab, spawnPosition, Quaternion.identity);
                
        island.AddComponent<IslandMover>().Init(fallSpeed, destroyY);

    }
}
