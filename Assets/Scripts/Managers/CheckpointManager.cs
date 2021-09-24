using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Transform checkpoint;

    #region Methods
    private void Start()
    {
        checkpoint = GameManager.GetInstance.checkpoint;

        if (PlayerPrefs.HasKey("checkpointX") && PlayerPrefs.HasKey("checkpointY"))
        {
            // sets initial position for checkpoint if player has prefs
            Vector3 initialPosition;
            initialPosition = new Vector3(PlayerPrefs.GetFloat("checkpointX"), PlayerPrefs.GetFloat("checkpointY"), checkpoint.position.z);
            checkpoint.position = initialPosition;
        }
    }

    public void MoveCheckpoint(Transform destination)
    {
        // moves checkpoint
        checkpoint.position = destination.position;
    }
    #endregion
}
