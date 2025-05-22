using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Island Generation Settings

    [Header("Island Settings")]
    public GameObject[] islandPrefabs;      // Prefab variants to spawn
    public float spawnInterval = 4f;        // TODO: Randomize this and avoid overlapping spawns
    public float fallSpeed = 2f;
    public float spawnXMin = -8f;
    public float spawnXMax = 8f;
    public float spawnY = 10f;
    public float destroyY = -10f;
    public float tileSize = 1f;             // 1 unit = 1 tile (16 pixels)
    public int minSlotX = -8;               // Leftmost tile slot index
    public int maxSlotX = 8;                // Rightmost tile slot index

    //-----------------------------------------------------------------------------//
    // Private Fields

    private float timer;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnIsland();
            timer = 0f;
        }
    }

    //-----------------------------------------------------------------------------//
    // Island Spawning Logic

    void SpawnIsland()
    {
        if (islandPrefabs.Length == 0) return;

        // Select a random prefab
        GameObject prefab = islandPrefabs[Random.Range(0, islandPrefabs.Length)];

        // Snap island to tile-based grid
        int slotIndex = Random.Range(minSlotX, maxSlotX + 1);
        float snappedX = slotIndex * tileSize;

        Vector2 spawnPosition = new Vector2(snappedX, spawnY);
        GameObject island = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Attach movement behavior
        island.AddComponent<IslandMover>().Init(fallSpeed, destroyY);
    }
}
