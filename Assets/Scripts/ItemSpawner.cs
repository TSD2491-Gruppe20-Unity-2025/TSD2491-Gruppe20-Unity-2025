using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Item Spawning Settings

    [Header("Item Settings")]
    public GameObject[] itemPrefabs;       // Prefab variants to spawn
    public float spawnInterval = 3f;       // TODO: Randomize this and avoid overlapping spawns
    public float fallSpeed = 3f;
    public float spawnXMin = -8f;
    public float spawnXMax = 8f;
    public float spawnY = 10f;
    public float destroyY = -10f;
    public float tileSize = 1f;            // 1 unit = 1 tile
    public int minSlotX = -8;
    public int maxSlotX = 8;

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
            SpawnItem();
            timer = 0f;
        }
    }

    //-----------------------------------------------------------------------------//
    // Item Spawning Logic

    void SpawnItem()
    {
        if (itemPrefabs.Length == 0) return;

        // Select a random item prefab
        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        // Snap item to tile-based grid
        int slotIndex = Random.Range(minSlotX, maxSlotX + 1);
        float snappedX = slotIndex * tileSize;

        Vector2 spawnPosition = new Vector2(snappedX, spawnY);
        GameObject item = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Attach movement behavior
        item.AddComponent<IslandMover>().Init(fallSpeed, destroyY);
    }
}
