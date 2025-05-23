using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMusicPlayer : MonoBehaviour
{
    void Start()
    {
        Debug.Log("IntroMusicPlayer is running...");

        if (SFXManager.Instance != null)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            SFXManager.Instance.StopMusic(); // Stop leftover music if any

            switch (sceneIndex)
            {
                case 0: // Intro video
                    SFXManager.Instance.PlayMusic(SFXEvent.Level1S);
                    break;
                case 1: // Main Menu
                    SFXManager.Instance.PlayMusic(SFXEvent.IntroMusicS);
                    break;
            }

            Debug.Log("Correct music triggered.");
        }
        else
        {
            Debug.LogWarning("SFXManager not found in scene!");
        }
    }
}
