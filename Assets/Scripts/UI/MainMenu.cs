using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;
    /// <summary>
    /// calls function in gamemanager
    /// for clicking in the button
    /// </summary>
    private void Start()
    {
        audioManager = AudioManager.GetInstance;
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu")
        {
            // otra condicion puesta a ultimo momento... 😎
            audioManager.FadeMusic(AudioManager.BackgroundMusic.MainMenuMusic);
        }
    }
    public void PauseMenu()
    {
        GameManager.GetInstance.PauseGame();
    }

    /// <summary>
    /// quits game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
