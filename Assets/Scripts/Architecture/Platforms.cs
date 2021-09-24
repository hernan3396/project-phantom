using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider2D))]

public class Platforms : MonoBehaviour
{
    #region Variables
    public PlayerMovement playerMovement; // temp fix
    public float duration = 0.3f;
    private bool isPlayerOn = false;
    private BoxCollider2D col2d;
    #endregion

    #region Methods
    private void Awake()
    {
        col2d = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // last part is a temp fix may fix later
        if (isPlayerOn && Input.GetKeyDown(KeyCode.S) && playerMovement.isActiveAndEnabled)
        {
            StartCoroutine(DisableCollider());
        }
    }

    public void PlayerIsOn()
    {
        isPlayerOn = !isPlayerOn;
    }

    /// <summary>
    /// disables collider to jump down
    /// </summary>
    IEnumerator DisableCollider()
    {
        col2d.enabled = false;
        yield return new WaitForSeconds(0.3f);
        col2d.enabled = true;
    }
    #endregion
}
