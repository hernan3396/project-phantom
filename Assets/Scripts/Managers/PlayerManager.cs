using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    #region Enumerators
    public enum Players
    {
        HumanPlayer = 0,
        GhostPlayer = 1
    }
    #endregion

    #region Variables
    #region Setup
    public bool onTutorial = false;
    private CinemachineController myCinemachine;
    private KeyCode changeCharactersButton;
    private KeyCode actionButton;
    #endregion
    #region PlayerSetup
    public PlayerMovement[] players = new PlayerMovement[2];
    public Transform[] playersPosition = new Transform[2];
    public SpriteRenderer ghostSprite;
    public Collider2D ghostHitbox;
    public float followSpeed = 5f;
    public PlayerJump humanJump;
    public GameObject ghost; // for possessing objects
    private Vector2 followHumanOffset = new Vector2(1, 0.5f);
    private bool isCurrentHuman = true;
    #endregion
    private AudioManager audioManager;
    private Transform checkpoint;
    #endregion

    #region Methods
    private void Start()
    {
        changeCharactersButton = KeybindingsManager.GetInstance.GetChangeCharactersButton;
        actionButton = KeybindingsManager.GetInstance.GetJumpButton;
        myCinemachine = GameManager.GetInstance.cinemachineController;

        checkpoint = GameManager.GetInstance.checkpoint;
        audioManager = AudioManager.GetInstance;
        if (!onTutorial) MovePlayers(checkpoint); // sets initial position

        GameManager.GetInstance.onDeath += OnDeath;
    }

    private void Update()
    {
        if (Input.GetKeyDown(changeCharactersButton))
        {
            ChangePlayerControl();
        }
    }

    private void LateUpdate()
    {
        if (isCurrentHuman)
        {
            FollowHumanPlayer();
        }
    }

    public void MovePlayers(Transform destination)
    {
        // if you go through a door while possessing this makes the ghost move too
        // because it was inactive while possessing
        if (!ghost.activeSelf)
        {
            DisableGhost();
        }
        // moves players back to destination (usually checkpoint)
        foreach (Transform player in playersPosition)
        {
            player.position = destination.position;
        }
    }

    /// <summary>
    /// resets player after a while
    /// on hit
    /// </summary>
    public void OnDeath(float duration)
    {
        StartCoroutine(DeathTransition(duration));
    }

    /// <summary>
    /// corrutine for OnDeath();
    /// </summary>
    IEnumerator DeathTransition(float duration)
    {
        yield return new WaitForSeconds(duration);
        MovePlayers(checkpoint); // moves player to checkpoint
    }

    public void DisableGhost()
    {
        // activates or desactivates ghost
        // for possessing (most of the time)
        ghost.SetActive(!ghost.activeSelf);
    }

    public void ChangePlayerControl()
    {
        // stops/starts movement script in player
        // and changes isCurrentHuman
        // checks if ghost is active in case you are
        // possessing 
        if (ghost.activeInHierarchy)
        {
            foreach (PlayerMovement player in players)
            {
                player.enabled = !player.enabled; // stops movement script

                // changes follow in cinemachine if cinemachine is avalible
                if (player.enabled && (myCinemachine != null))
                {
                    myCinemachine.ChangeFollow(player.transform);
                }
            }
            humanJump.enabled = !humanJump.enabled; // stops jumping script
            isCurrentHuman = !isCurrentHuman;

            ChangeMusic();

            // enables/disables hitbox in case ghost was inside
            // a box (or something else) when changing controll to it
            ghostHitbox.enabled = !isCurrentHuman;
        }
    }

    void ChangeMusic()
    {
        if (isCurrentHuman) audioManager.FadeMusic(AudioManager.BackgroundMusic.HumanMusic);
        else audioManager.FadeMusic(AudioManager.BackgroundMusic.GhostMusic);
    }

    void FollowHumanPlayer()
    {
        /* makes ghost follow player with a little delay */
        Transform humanPosition = playersPosition[(int)Players.HumanPlayer];
        Transform ghostPosition = playersPosition[(int)Players.GhostPlayer];

        float t = Time.deltaTime * followSpeed;
        int relativeDirection = 1; // no supe que otro nombre ponerle pero creo que este sirve

        if (humanPosition.position.x >= ghostPosition.position.x)
        {
            relativeDirection *= -1;
            ghostSprite.flipX = false;
        }
        else ghostSprite.flipX = true;

        float xLerp = Mathf.Lerp(ghostPosition.position.x, humanPosition.position.x + (relativeDirection * followHumanOffset.x), t);
        float yLerp = Mathf.Lerp(ghostPosition.position.y, humanPosition.position.y + followHumanOffset.y, t);

        ghostPosition.position = new Vector3(xLerp, yLerp, ghostPosition.position.z);
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onDeath -= OnDeath;
    }
    #endregion
}
