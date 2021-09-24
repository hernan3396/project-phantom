using UnityEngine;

public class Curtains : MonoBehaviour
{
    #region Variables
    public bool curtainState = false;
    public Animator transition;
    #endregion

    #region Methods
    /// <summary>
    /// triggers transition
    /// </summary>
    public void OnGhostTrigger()
    {
        curtainState = !curtainState;
        transition.SetBool("isGhostIn", curtainState);
    }
    #endregion
}
