using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private AudioManager audioManager;
    /// <summary>
    /// loads scene
    /// </summary>

    private void Start()
    {
        audioManager = AudioManager.GetInstance;
    }

    public void LoadScene(string value)
    {
        if (value == "GameOver") audioManager.FadeMusic(AudioManager.BackgroundMusic.GameOver); // estas ultimas cosas son tan shady pero bue 😎
        SceneManager.LoadScene(value);
    }

    /// <summary>
    /// checks if player has a save
    /// if not starts from level 0
    /// </summary>
    public void PlayGame()
    {
        string value = "level_000"; // default : first level
        if (PlayerPrefs.HasKey("scene"))
        {
            value = PlayerPrefs.GetString("scene");
        }
        audioManager.FadeMusic(AudioManager.BackgroundMusic.HumanMusic);
        LoadScene(value);
    }
}
