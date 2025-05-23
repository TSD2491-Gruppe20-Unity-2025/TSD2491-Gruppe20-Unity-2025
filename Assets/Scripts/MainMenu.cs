using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // UI Floating Logo

    [Header("Floating Logo Settings")]
    [SerializeField] private RectTransform logoTransform;
    [SerializeField] private float floatAmplitude = 20f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector2 logoStartPos;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    void Start()
    {
        if (logoTransform != null)
        {
            logoStartPos = logoTransform.anchoredPosition;
        }
    }

    void Update()
    {
        if (logoTransform != null)
        {
            float newY = logoStartPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            logoTransform.anchoredPosition = new Vector2(logoStartPos.x, newY);
        }
    }

    //-----------------------------------------------------------------------------//
    // Public Methods

    public void PlayGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SFXManager.Instance.PlayUI(SFXEvent.StartButtonClickS);

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Next scene index out of range!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
