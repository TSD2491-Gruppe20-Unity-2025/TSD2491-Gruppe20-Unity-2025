using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Public Methods

    public void PlayGame()
    {
        // Load the next scene in the build index
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

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
        // Exit the application
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}