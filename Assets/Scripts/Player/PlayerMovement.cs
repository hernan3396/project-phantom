using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    #region Setup
    public SpriteRenderer spriteRenderer;
    public float movementSpeed;
    public bool isPlayable;
    public bool isGhost;
    private AudioManager audioManager;
    private CapsuleCollider2D col2D;
    private Rigidbody2D rb2D;
    #endregion
    #region Movement
    private bool stopMovement = false;
    private Vector2 playerMomentum;
    private float hMovement;
    private float vMovement;
    #endregion
    #endregion

    #region Methods
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = spriteRenderer.GetComponent<SpriteRenderer>();

        if (isPlayable)
        {
            col2D = GetComponent<CapsuleCollider2D>();
        }
    }

    private void Start()
    {
        audioManager = AudioManager.GetInstance;

        GameManager.GetInstance.onGamePaused += PauseResume;
        GameManager.GetInstance.onDeath += OnDeath;
    }

    private void Update()
    {
        if (stopMovement) return;

        hMovement = Input.GetAxisRaw("Horizontal");

        FlipSprite();

        if (isGhost)
        {
            // if character is ghost you can move vertically
            vMovement = Input.GetAxisRaw("Vertical");

            if (hMovement != 0 || vMovement != 0)
            {
                audioManager.WalkingSFX(AudioManager.CharacterSFX.GhostFloating);
            }
        }
    }

    private void FixedUpdate()
    {
        if (stopMovement) return;
        // separated to avoid errors (player cant move vertically)
        if (isGhost)
        {
            GhostMovement();
        }
        else
        {
            HumanMovement();
        }
    }

    /// <summary>
    /// manages death animation for playable characters
    /// </summary>
    public void OnDeath(float duration)
    {
        if (isPlayable && this.enabled)
        {
            StartCoroutine(PlayerDeath(duration)); // starts corrutine for death animation
        }
    }

    /// <summary>
    /// little animation for human and ghost
    /// </summary>
    IEnumerator PlayerDeath(float duration)
    {
        AnimationSetup();
        yield return new WaitForSeconds(duration);
        AnimationSetup();
    }

    /// <summary>
    /// setup for the animation
    /// </summary>
    void AnimationSetup()
    {
        stopMovement = !stopMovement;
        col2D.enabled = !col2D.enabled;

        if (stopMovement)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            // characters little animation (no need to separate this in a diff method)
            if (isGhost) spriteRenderer.color = new Color(1, 1, 1, 0.2f);
            else rb2D.AddForce(Vector2.up * 600);
        }
        else
        {
            rb2D.velocity = Vector3.zero;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (isGhost) spriteRenderer.color = Color.white; // removes? ghost transparency
        }
    }

    void FlipSprite()
    {
        // flips sprite depending on direction
        if (hMovement < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (hMovement > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void GhostMovement()
    {
        rb2D.velocity = new Vector2(hMovement, vMovement) * movementSpeed;
    }

    void HumanMovement()
    {
        rb2D.velocity = new Vector2(hMovement * movementSpeed, rb2D.velocity.y);
    }

    /// <summary>
    /// stores player momentum before pause
    /// and gives it to player after pause
    /// </summary>
    public void PauseResume(bool gamePaused)
    {
        if (gamePaused)
        {
            playerMomentum = rb2D.velocity;
            rb2D.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.velocity = playerMomentum;
        }
        stopMovement = gamePaused;
    }

    /// <summary>
    /// stops ghost velocity
    /// mostly used when changing characters
    /// </summary>
    void StopGhost()
    {
        rb2D.velocity = Vector2.zero;
    }

    private void OnDisable()
    {
        // stops ghost momentum when switching
        if (isGhost)
        {
            StopGhost();
        }
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
        GameManager.GetInstance.onDeath -= OnDeath;
    }
    #endregion
}
