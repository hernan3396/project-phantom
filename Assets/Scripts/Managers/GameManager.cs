using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    #region Pause
    public delegate void OnGamePaused(bool paused);
    public event OnGamePaused onGamePaused;
    private bool gamePaused = false;
    private KeyCode pauseButton;
    #endregion
    #region Prompt
    public Vector3 vectorOff = new Vector3(0, 1, 0);
    public GameObject prompt;
    #endregion
    #region Death
    public delegate void OnDeath(float duration);
    public GameObject deathTransition;
    public event OnDeath onDeath;
    public float duration = 1.2f;
    public float deathAnimExtension = 0.3f;
    private AudioManager audioManager;
    #endregion
    #region Dialogues
    [Header("Dialogues")]
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public Dialogue[] dialogues;
    public TMP_Text keyText;
    #endregion
    public CinemachineController cinemachineController;
    public Transform checkpoint;
    private static GameManager instance;
    #endregion
    #region Methods
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        pauseButton = KeybindingsManager.GetInstance.GetPauseButton;
        audioManager = AudioManager.GetInstance;

        if (dialogues != null)
        {
            foreach (Dialogue dialogue in dialogues)
            {
                dialogue.showDialogue += ShowDialogue;
            }
        }
    }

    /// <summary>
    /// a delegate, recieves text(dialogue) and a key(to press)
    /// and shows it on screen
    /// </summary>
    public void ShowDialogue(TextAsset text, TextAsset key)
    {
        dialogueText.text = text.text;

        if (key != null) keyText.text = "Press " + key.text;
        else keyText.text = "";

        dialogueBox.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // pauses game using delegates
        gamePaused = !gamePaused;

        if (onGamePaused != null)
        {
            ManageMenu();
            onGamePaused(gamePaused);
        }
    }

    /// <summary>
    /// invokes? this delegate
    /// also it calls a little animation
    /// </summary>
    public void PlayerDeath()
    {
        if (onDeath != null)
        {
            onDeath(duration);
        }
        audioManager.PlayCharSFX(AudioManager.CharacterSFX.Death);
        DeathScreen(); // starts animation
        Invoke("DeathScreen", duration + deathAnimExtension); // stops animation (little delay to avoid camera glitches) magic numbers
    }

    /// <summary>
    /// a little black screen covers the screen
    /// is not the best but it works
    /// </summary>
    private void DeathScreen()
    {
        deathTransition.SetActive(!deathTransition.activeInHierarchy);
    }

    /// <summary>
    /// couldnt find a better place to make this method
    /// and didnt want to create a script for a single method
    /// so it is here 😎 (it kinda makes sense)
    /// </summary>
    public void MovePrompt(Transform objective)
    {
        prompt.transform.position = objective.transform.position + vectorOff;
        prompt.SetActive(!prompt.activeInHierarchy);
    }

    /// <summary>
    /// loads/unloads Game menu scene
    /// </summary>
    void ManageMenu()
    {
        if (SceneManager.GetSceneByName("GameMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("GameMenu");
        }
        else
        {
            SceneManager.LoadSceneAsync("GameMenu", LoadSceneMode.Additive);
        }
    }

    void OnDestroy()
    {
        if (instance != this)
        {
            instance = null;
        }
    }

    // set & get
    public static GameManager GetInstance
    {
        get { return instance; }
    }
    #endregion
}
