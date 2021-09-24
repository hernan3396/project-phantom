using UnityEngine;
using UnityEngine.Events;

public class OnPlayerEnter : MonoBehaviour
{
    #region Variables
    public UnityEvent onPlayerInside;
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;
    public UnityEvent onGhostEnter;
    public UnityEvent onGhostExit;
    private bool isPlayerInside = false;
    #endregion

    #region Methods
    public void PlayerInside()
    {
        isPlayerInside = !isPlayerInside;
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            onPlayerInside?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsHuman(other.gameObject))
        {
            onPlayerEnter?.Invoke();
        }
        else if (IsGhost(other.gameObject))
        {
            onGhostEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsHuman(other.gameObject))
        {
            onPlayerExit?.Invoke();
        }
        else if (IsGhost(other.gameObject))
        {
            onGhostExit?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsHuman(other.gameObject))
        {
            onPlayerEnter?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (IsHuman(other.gameObject))
        {
            onPlayerExit?.Invoke();
        }
    }

    private bool IsHuman(GameObject other)
    {
        return other.CompareTag("HumanPlayer");
    }

    private bool IsGhost(GameObject other)
    {
        return other.CompareTag("GhostPlayer");
    }
    #endregion
}
