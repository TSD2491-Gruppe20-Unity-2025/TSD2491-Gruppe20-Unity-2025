using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("UI")]
    public UIScript player1UI;
    public UIScript player2UI;

    [Header("Enemy and World Generation")]
    public GameObject enemySpawnerPrefab;
    public GameObject islandGeneratorPrefab;

    private PlayerController player1;
    private PlayerController player2;
    private GameObject enemySpawner;
    private GameObject islandGenerator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupWorld();
        SpawnPlayers();
    }

    private void SetupWorld()
    {
        // Spawn/Activate island generator
        if (islandGeneratorPrefab != null)
        {
            islandGenerator = Instantiate(islandGeneratorPrefab, Vector3.zero, Quaternion.identity);
        }

        // Spawn/Activate enemy spawner
        if (enemySpawnerPrefab != null)
        {
            enemySpawner = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    private void SpawnPlayers()
    {
        Vector2 player1Start = new Vector2(-3f, -10f);
        Vector2 player1Target = new Vector2(-3f, 0f);

        GameObject p1 = Instantiate(playerPrefab, player1Start, Quaternion.identity);
        player1 = p1.GetComponent<PlayerController>();
        player1.AssignUI(player1UI);
        player1.StartFlyIn(player1Target);
    }

    public void CheckGameOver()
    {
        if (player1.CurrentHealth <= 0 && player2.CurrentHealth <= 0)
        {
            Debug.Log("Both players dead. Game Over.");
            Time.timeScale = 0f;
        }
    }
}
