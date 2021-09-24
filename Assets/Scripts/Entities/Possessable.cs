using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(PlayerMovement))]

public class Possessable : MonoBehaviour
{
    // no lo pude hacer usando solo OnPlayerEnter
    // ya que desactivas un rato al fantasma
    // asi que tuve que usar un "isOnRange"
    #region Variables
    #region Possess
    public UnityEvent onPossess;
    private bool isGhostOnRange = false;
    private bool isPossessing = false;
    #endregion
    #region Setup
    private CinemachineController myCinemachine;
    private PlayerMovement movement;
    private KeyCode actionButton;
    private float gravitiScale;
    private Rigidbody2D rb2D;
    #endregion
    #endregion

    #region Methods
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb2D = GetComponent<Rigidbody2D>();
        gravitiScale = rb2D.gravityScale;

        actionButton = KeybindingsManager.GetInstance.GetJumpButton;
        myCinemachine = GameManager.GetInstance.cinemachineController;

        GameManager.GetInstance.onDeath += OnDeath;
    }

    private void Update()
    {
        if ((isPossessing || isGhostOnRange) && Input.GetKeyDown(actionButton))
        {
            onPossess.Invoke();
        }
    }

    /// <summary>
    /// checks if ghost is on range
    /// </summary>
    public void GhostOnRange()
    {
        isGhostOnRange = !isGhostOnRange;
    }

    /// <summary>
    /// manages death if ghost was possessing something
    /// else it does nothing
    /// </summary>
    private void OnDeath(float value)
    {
        if (isPossessing)
        {
            onPossess.Invoke();
        }
    }

    /// <summary>
    /// possess object
    /// </summary>
    public void Possessed()
    {
        movement.enabled = !movement.enabled;
        isPossessing = !isPossessing;
        rb2D.freezeRotation = movement.enabled; // freezes rotation

        // removes gravity or gives it back
        if (movement.enabled)
        {
            rb2D.gravityScale = 0;
            ChangeFollow(this.transform);
        }

        else rb2D.gravityScale = gravitiScale;
    }

    /// <summary>
    /// when you stop possessing moves
    /// ghost to object last position
    /// </summary>
    public void MoveGhost(Transform ghost)
    {
        // moves ghost to box position
        if (!isPossessing)
        {
            ghost.position = transform.position;
            ChangeFollow(ghost);
        }
    }

    /// <summary>
    /// summons ChangeFollow from CinemachineController
    /// </summary>
    void ChangeFollow(Transform objective)
    {
        if (myCinemachine != null) myCinemachine.ChangeFollow(objective);

    }
    #endregion

}
