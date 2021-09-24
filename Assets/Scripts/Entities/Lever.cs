using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    #region Variables
    public UnityEvent onActivation;
    public Sprite leverOff;
    private SpriteRenderer spriteRenderer;
    private KeyCode actionButton;
    #endregion

    #region Methods
    private void Start()
    {
        actionButton = KeybindingsManager.GetInstance.GetJumpButton;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// activates it and disables it so you can't use it again
    /// </summary>
    public void Activate()
    {
        if (Input.GetKeyDown(actionButton))
        {
            spriteRenderer.sprite = leverOff; // changes sprite
            this.enabled = false; // disables script
            onActivation?.Invoke();
        }
    }
    #endregion
}
