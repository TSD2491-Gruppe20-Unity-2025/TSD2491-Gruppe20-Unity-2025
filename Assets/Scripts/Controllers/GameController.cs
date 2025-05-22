using UnityEngine;

public class GameController : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Singleton Instance

    public static GameController Instance { get; private set; }

    //-----------------------------------------------------------------------------//
    // Inspector Variables

    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("UI")]
    public UIScript player1UI;
    public UIScript player2UI;

    [Header("Enemy and World Generation")]
    public GameObject enemySpawnerPrefab;
    public GameObject islandGeneratorPrefab;
    public GameObject itemGeneratorPrefab;

    //-----------------------------------------------------------------------------//
    // Private Fields

    private PlayerController player1;
    private PlayerController player2;
    private GameObject enemySpawner;
    private GameObject islandGenerator;
    private GameObject itemGenerator;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    private void Awake()
    {
        // Ensure singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupWorld();
        SpawnPlayers();
    }

    //-----------------------------------------------------------------------------//
    // World Setup

    private void SetupWorld()
    {
        // Instantiate island generator
        if (islandGeneratorPrefab != null)
        {
            islandGenerator = Instantiate(islandGeneratorPrefab, Vector3.zero, Quaternion.identity);
        }

        // Instantiate Item generator
        if (itemGeneratorPrefab != null)
        {
            itemGenerator = Instantiate(itemGeneratorPrefab, Vector3.zero, Quaternion.identity);
        }

        // Instantiate enemy spawner
        if (enemySpawnerPrefab != null)
        {
            enemySpawner = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    //-----------------------------------------------------------------------------//
    // Player Spawning

    private void SpawnPlayers()
    {
        Vector2 player1Start = new Vector2(-3f, -10f);
        Vector2 player1Target = new Vector2(-3f, 0f);

        GameObject p1 = Instantiate(playerPrefab, player1Start, Quaternion.identity);
        player1 = p1.GetComponent<PlayerController>();
        player1.AssignUI(player1UI);
        player1.StartFlyIn(player1Target);

        // Optionally spawn player 2 here
        // Example setup if needed:
        // Vector2 player2Start = new Vector2(3f, -10f);
        // Vector2 player2Target = new Vector2(3f, 0f);
        // GameObject p2 = Instantiate(playerPrefab, player2Start, Quaternion.identity);
        // player2 = p2.GetComponent<PlayerController>();
        // player2.AssignUI(player2UI);
        // player2.StartFlyIn(player2Target);
    }

    //-----------------------------------------------------------------------------//
    // Game Over Check

    public void CheckGameOver()
    {
        if (player1.CurrentHealth <= 0 && player2.CurrentHealth <= 0)
        {
            Debug.Log("Both players dead. Game Over.");
            Time.timeScale = 0f;
        }
    }
}
