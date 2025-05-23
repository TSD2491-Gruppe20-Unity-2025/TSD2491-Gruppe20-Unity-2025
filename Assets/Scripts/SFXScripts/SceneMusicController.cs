using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicController : MonoBehaviour
{
    void Start()
    {
        if (SFXManager.Instance == null)
        {
            Debug.LogWarning("SFXManager not found!");
            return;
        }

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("SceneMusicController running in scene " + sceneIndex);

        SFXManager.Instance.StopMusic(); // Stop any previous scene's music

        switch (sceneIndex)
        {
            case 0: // Intro scene with video
                SFXManager.Instance.PlayMusic(SFXEvent.Level1S);
                break;
            case 1: // Main menu
                SFXManager.Instance.PlayMusic(SFXEvent.IntroMusicS);
                break;
            case 2:
                SFXManager.Instance.PlayMusic(SFXEvent.Level1S);
                break;
            case 3:
                SFXManager.Instance.PlayMusic(SFXEvent.Level2S);
                break;
            case 4:
                SFXManager.Instance.PlayMusic(SFXEvent.Level3S);
                break;
            case 5:
                SFXManager.Instance.PlayMusic(SFXEvent.Level4S);
                break;
            case 6:
                SFXManager.Instance.PlayMusic(SFXEvent.Level5S);
                break;
            case 7: // Outro
                SFXManager.Instance.PlayMusic(SFXEvent.OutroMusicS);
                break;
            default:
                Debug.LogWarning("Scene not handled in SceneMusicController");
                break;
        }
    }
}
