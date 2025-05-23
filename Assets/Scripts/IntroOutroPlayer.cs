using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

    }
}
