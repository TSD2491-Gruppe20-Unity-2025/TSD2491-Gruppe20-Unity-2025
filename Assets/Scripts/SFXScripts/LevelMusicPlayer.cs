using UnityEngine;

public class LevelMusicPlayer : MonoBehaviour
{
    void Start()
    {
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            case 1: // Assuming Level 1 is at build index 1
                SFXManager.Instance.PlayMusic(SFXEvent.Level1S);
                break;
            case 2:
                SFXManager.Instance.PlayMusic(SFXEvent.Level2S);
                break;
            case 3:
                SFXManager.Instance.PlayMusic(SFXEvent.Level3S);
                break;
            case 4:
                SFXManager.Instance.PlayMusic(SFXEvent.Level4S);
                break;
            case 5:
                SFXManager.Instance.PlayMusic(SFXEvent.Level5S);
                break;
        }
    }
}
