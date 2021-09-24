using UnityEngine;

public class KeybindingsManager : MonoBehaviour
{
    // manages keybindings
    #region Variables
    #region KeyCodes
    private KeyCode changeCharactersButton = KeyCode.LeftShift;
    private KeyCode pauseButton = KeyCode.Escape;
    private KeyCode jumpButton = KeyCode.Space;
    #endregion
    private static KeybindingsManager instance;
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

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    // set & get
    public static KeybindingsManager GetInstance
    {
        get { return instance; }
    }

    public KeyCode GetJumpButton
    {
        get { return jumpButton; }
    }

    public KeyCode GetChangeCharactersButton
    {
        get { return changeCharactersButton; }
    }

    public KeyCode GetPauseButton
    {
        get { return pauseButton; }
    }
    #endregion
}