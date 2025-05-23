using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Player Prefab")]
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

    [Header("UI")]
    public UIScript player1UI;
    public UIScript player2UI;
    public GameObject gameOverPanel;
    public TextMeshProUGUI creditsText;

    [Header("Score UI (in-game)")]
    public TextMeshProUGUI player1ScoreTextUI;
    public TextMeshProUGUI player2ScoreTextUI;

    [Header("Final Score UI (game over screen)")]
    public TextMeshProUGUI endScreenTitleText;
    public TextMeshProUGUI player1FinalScoreText;
    public TextMeshProUGUI player2FinalScoreText;

    [Header("Enemy and World Generation")]
    public GameObject enemySpawnerPrefab;
    public GameObject islandGeneratorPrefab;
    public GameObject itemGeneratorPrefab;


    [Header("Player Spawn Positions")]
    public Vector2 player1Target = new Vector2(-3f, 0f);
    public Vector2 player2Target = new Vector2(3f, 0f);

    private PlayerController player1;
    private PlayerController player2;
    private GameObject enemySpawner;
    private GameObject islandGenerator;

    private GameObject itemGenerator;

    private int player1Score = 0;
    private int player2Score = 0;

    private bool player1Spawned = false;
    private bool player2Spawned = false;

    private bool bossDefeated = false;
    private int credits = 0;

    public PlayerController Player1 => player1;
    public PlayerController Player2 => player2;

 private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
    {
        Destroy(gameObject);
        return;
    }

    credits = GameDataManager.Instance.credits;
    player1Score = GameDataManager.Instance.scoreP1;
    player2Score = GameDataManager.Instance.scoreP2;

    SetupWorld();
    UpdateCreditsUI();
        if (player1ScoreTextUI != null) player1ScoreTextUI.text = $"P1 Score: {player1Score}";
        if (player2ScoreTextUI != null) player2ScoreTextUI.text = $"P2 Score: {player2Score}";

        // Spawn alive players immediately for free
        if (GameDataManager.Instance.player1Alive)
        {
            credits += 1;
            SpawnPlayer1();
        }
        // modify SpawnPlayer1() to not deduct credits for free spawn

        if (GameDataManager.Instance.player2Alive)
        {
            credits += 1;
            SpawnPlayer2();  // same here
        }
}

    private void Update()
    {
        HandleCredits();
        HandlePlayerSpawning();
    }

    private void SetupWorld()
    {
        if (islandGeneratorPrefab != null)
            islandGenerator = Instantiate(islandGeneratorPrefab, Vector3.zero, Quaternion.identity);
        if (enemySpawnerPrefab != null)
            enemySpawner = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity);
        if (itemGeneratorPrefab != null)
            itemGenerator = Instantiate(itemGeneratorPrefab, Vector3.zero, Quaternion.identity);
    }

    private void HandleCredits()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            credits++;
            UpdateCreditsUI();
        }
    }

    private void UpdateCreditsUI()
    {
        if (creditsText != null)
            creditsText.text = $"CREDITS: {credits}";
    }

    private void HandlePlayerSpawning()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player1 == null || player1.CurrentHealth <= 0)
            {
                if (credits > 0)
                {
                    credits--;
                    UpdateCreditsUI();
                    SpawnPlayer1();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player2 == null || player2.CurrentHealth <= 0)
            {
                if (credits > 0)
                {
                    credits--;
                    UpdateCreditsUI();
                    SpawnPlayer2();
                }
            }
        }
    }

    private void SpawnPlayer1()
    {
        Vector2 start = new Vector2(-3f, -10f);
        GameObject p1 = Instantiate(playerOnePrefab, start, Quaternion.identity);
        player1 = p1.GetComponent<PlayerOne>();
        player1.AssignUI(player1UI);
        player1.StartFlyIn(player1Target);
        player1Spawned = true;
    }

    private void SpawnPlayer2()
    {
        Vector2 start = new Vector2(3f, -10f);
        GameObject p2 = Instantiate(playerTwoPrefab, start, Quaternion.identity);
        player2 = p2.GetComponent<PlayerTwo>();
        player2.AssignUI(player2UI);
        player2.StartFlyIn(player2Target);
        player2Spawned = true;
    }

    public void RegisterEnemyKill(PlayerController killer)
    {
        if (killer == player1)
        {
            player1Score++;
            if (player1ScoreTextUI != null)
                player1ScoreTextUI.text = $"P1 Score: {player1Score}";
        }
        else if (killer == player2)
        {
            player2Score++;
            if (player2ScoreTextUI != null)
                player2ScoreTextUI.text = $"P2 Score: {player2Score}";
        }
    }

    public void OnBossDefeated()
    {
        if (!bossDefeated)
        {
            bossDefeated = true;
            ShowFinalScore(true);
        }
    }

    private void ShowFinalScore(bool levelCompleted)
    {
        if (gameOverPanel != null)
        {
            if (endScreenTitleText != null)
                endScreenTitleText.text = levelCompleted ? "Level Complete! Loading next level..." : "Game Over";

            if (player1FinalScoreText != null)
                player1FinalScoreText.text = "Player 1 Score: " + player1Score;

            if (player2FinalScoreText != null)
                player2FinalScoreText.text = "Player 2 Score: " + player2Score;

            if (levelCompleted)
                StartCoroutine(AutoAdvanceToNextLevel());
        }
    }

    private IEnumerator AutoAdvanceToNextLevel()
    {
        GameDataManager.Instance.credits = credits;
        GameDataManager.Instance.scoreP1 = player1Score;
        GameDataManager.Instance.scoreP2 = player2Score;
        if(player1 != null) GameDataManager.Instance.player1Alive = player1.CurrentHealth > 0;
        if(player2 != null) GameDataManager.Instance.player2Alive = player2.CurrentHealth > 0;
        yield return new WaitForSecondsRealtime(10f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextLevel()
    {
        SFXManager.Instance.StopMusic();

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            endScreenTitleText.text = "You have vanquished all enemies!";
            Debug.Log("No more levels to load.");

            SFXManager.Instance.Play(SFXEvent.VictoryS);
        }
    }
}
