using UnityEngine;

public class IntroMusicPlayer : MonoBehaviour
{
void Start()
{
    Debug.Log("IntroMusicPlayer is running...");

    if (SFXManager.Instance != null)
    {
        Debug.Log("Trying to play IntroMusicS...");
        SFXManager.Instance.PlayMusic(SFXEvent.IntroMusicS);
    }
    else
    {
        Debug.LogWarning("SFXManager not found in scene!");
    }
}
}
