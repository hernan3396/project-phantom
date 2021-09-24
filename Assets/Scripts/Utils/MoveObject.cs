using UnityEngine;

public class MoveObject : MonoBehaviour
{
    #region Variables
    public bool isMoving = false;
    public Transform objective;
    public Transform finalPos;
    public bool playMovSound;
    public int speed = 5;
    AudioManager audioManager;
    #endregion

    #region Methods
    private void Start()
    {
        audioManager = AudioManager.GetInstance;
    }
    private void Update()
    {
        if (isMoving)
        {
            MoveObjective();
        }
    }

    /// <summary>
    /// changes isMoving state
    /// </summary>
    public void IsMoving()
    {
        isMoving = !isMoving;
        if (playMovSound) audioManager.PlaySFX(AudioManager.SFX.BridgeStart);
    }

    /// <summary>
    /// moves objective to final pos
    /// </summary>
    public void MoveObjective()
    {
        Vector3 vector3 = Vector3.MoveTowards(objective.position, finalPos.position, speed * Time.deltaTime);
        objective.position = vector3;

        // stops excecuting if objective is on place
        if (objective.position == finalPos.position)
        {
            isMoving = false;
            if (playMovSound) audioManager.PlaySFX(AudioManager.SFX.BridgeStop);
        }
    }

    /// <summary>
    /// instantly moves objective to final position
    /// </summary>
    public void InstantMove()
    {
        objective.position = finalPos.position;
    }

    public void ForceStopSFX()
    {
        audioManager.SFXSource.Stop();
    }
    #endregion
}
