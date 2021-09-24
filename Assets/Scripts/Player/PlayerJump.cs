using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerJump : MonoBehaviour
{
    #region Enum
    enum Animations
    {
        Idle,
        Walking,
        Jumping_Rising,
        Jumping_Falling
    }
    #endregion
    #region Variables
    #region Setup
    public float walkingTreshold = 0.05f;
    public Animator animator;
    public float jumpForce;
    private string[] animations = { "Idle", "Walking", "Jumping_Rising", "Jumping_Falling" };
    private AudioManager audioManager;
    private KeyCode jumpButton;
    Queue<KeyCode> inputBuffer;
    private Rigidbody2D rb2D;
    #endregion
    #region VariableJump
    private bool releaseJump = false;
    private bool isOnGround = false;
    private bool startTimer = false;
    private float jumpTimer = 0.2f;
    private float gravityScale;
    private float timer;
    #endregion
    #region CoyoteTime
    public float coyoteFrames = 3f;
    public float coyoteTimer;
    private Vector3 raycastOffset = new Vector3(0.18f, 0, 0);
    private float raycastLenght = 0.7f;
    #endregion

    #endregion

    #region Methods
    private void Awake()
    {
        inputBuffer = new Queue<KeyCode>();
        rb2D = GetComponent<Rigidbody2D>();
        gravityScale = rb2D.gravityScale;
        timer = jumpTimer;
    }

    private void Start()
    {
        jumpButton = KeybindingsManager.GetInstance.GetJumpButton;
        audioManager = AudioManager.GetInstance;
    }

    private void Update()
    {
        ManageAnimations();

        if (isGroundColliding())
        {
            coyoteTimer = 0; // resets coyoteTimer
            isOnGround = true;
        }
        else
        {
            coyoteTimer += 1; // start adding to coyoteTimer
            isOnGround = false;
        }

        if (Input.GetKeyDown(jumpButton))
        {
            inputBuffer.Enqueue(jumpButton); // saves space to buffer
            Invoke("RemoveAction", 0.1f); // deletes action after 0.1f
        }

        // dynamic jump
        if ((isOnGround || coyoteTimer < coyoteFrames) && inputBuffer.Count > 0)
        {
            if (inputBuffer.Peek() == jumpButton)
            {
                // peeks into buffer to check for jumpButton
                inputBuffer.Clear(); // clears buffer when you jump to avoid double jump on the same frame
                Jump();
            }
        }

        if (Input.GetKeyUp(jumpButton))
        {
            releaseJump = true;
        }

        if (startTimer)
        {
            // stops jump
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                releaseJump = true;
            }
        }

        if (releaseJump)
        {
            StopJump();
        }
    }

    void ManageAnimations()
    {
        // no lo probe a fondo por si hay bugs pero mas o menos funciona
        if (isOnGround && Mathf.Abs(rb2D.velocity.y) <= walkingTreshold)
        {
            // la 2da condicion chequea es por si hay una caja o una plataforma mientras salta para que no quede rara la animacion
            if (Mathf.Abs(rb2D.velocity.x) >= walkingTreshold)
            {
                animator.Play("Walking");
                audioManager.WalkingSFX(AudioManager.CharacterSFX.Walking);
            }
            else animator.Play("Idle");
        }
        else
        {
            if (startTimer) animator.Play("Jumping_Rising");
            else animator.Play("Jumping_Falling");
        }
    }

    private bool isGroundColliding()
    {
        // checks if player is colliding with floor
        // and returns bool

        RaycastHit2D rayHit1 = Physics2D.Raycast(transform.position, Vector3.down, raycastLenght);
        RaycastHit2D rayHit2 = Physics2D.Raycast(transform.position + raycastOffset, Vector3.down, raycastLenght);
        RaycastHit2D rayHit3 = Physics2D.Raycast(transform.position - raycastOffset, Vector3.down, raycastLenght);

        bool isCollidingCenter = rayHit1.collider && (rayHit1.collider.CompareTag("Floor") || rayHit1.collider.CompareTag("Box"));
        bool isCollidingRight = rayHit2.collider && (rayHit2.collider.gameObject.CompareTag("Floor") || rayHit2.collider.CompareTag("Box"));
        bool isCollidingLeft = rayHit3.collider && (rayHit3.collider.gameObject.CompareTag("Floor") || rayHit3.collider.CompareTag("Box"));

        Debug.DrawRay(transform.position, Vector3.down * raycastLenght, Color.red);
        Debug.DrawRay(transform.position + raycastOffset, Vector3.down * raycastLenght, Color.red);
        Debug.DrawRay(transform.position - raycastOffset, Vector3.down * raycastLenght, Color.red);

        return isCollidingCenter || isCollidingLeft || isCollidingRight;
    }

    private void Jump()
    {
        isOnGround = false;
        audioManager.PlayCharSFX(AudioManager.CharacterSFX.Jump);

        rb2D.gravityScale = 0;
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(Vector2.up * jumpForce);
        startTimer = true;
        // animator.Play(animations[(int)Animations.Jumping_Rising]);
    }

    private void StopJump()
    {
        rb2D.gravityScale = gravityScale;
        releaseJump = false;
        timer = jumpTimer;
        startTimer = false;
        // animator.Play(animations[(int)Animations.Jumping_Falling]);
    }

    private void RemoveAction()
    {
        if (inputBuffer.Count > 0) inputBuffer.Dequeue();
    }

    private void OnDisable()
    {
        rb2D.gravityScale = gravityScale;
        if (animator.isActiveAndEnabled) animator.Play("Idle");
    }
    #endregion
}