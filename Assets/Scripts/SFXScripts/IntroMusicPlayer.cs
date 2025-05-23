using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMusicPlayer : MonoBehaviour
{
void Start()
{
    Debug.Log("IntroMusicPlayer is running...");

    if (SFXManager.Instance != null)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) SFXManager.Instance.PlayMusic(SFXEvent.Level5S);
        if(SceneManager.GetActiveScene().buildIndex == 1) SFXManager.Instance.PlayMusic(SFXEvent.IntroMusicS);
        if(SceneManager.GetActiveScene().buildIndex == 7) SFXManager.Instance.PlayMusic(SFXEvent.Level1S);
        Debug.Log("Trying to play IntroMusicS...");
    }
    else
    {
        Debug.LogWarning("SFXManager not found in scene!");
    }
}
}
