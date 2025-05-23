using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public int credits = 0;
    public int scoreP1 = 0;
    public int scoreP2 = 0;
    public bool player1Alive = false;
    public bool player2Alive = false;

    public int scenenumber = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Optionally, you can add some helper methods for saving/loading data,
    // but your GameController can directly read/write the public fields.
}
