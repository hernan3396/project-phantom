using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    #region Variables
    public int direction = 1;
    public float movementSpeed = 5f;
    private AudioManager audioManager;
    private bool stopMovement = false;
    #endregion

    #region Methods
    private void Start()
    {
        audioManager = AudioManager.GetInstance;

        GameManager.GetInstance.onGamePaused += PauseResume;
    }

    // aca antes era con un pingpong pero al usar Time.time daba muchos problemas
    // asi que en los ultimos dias lo cambie a como esta ahora, por eso esta un poco desprolijo

    private void Update()
    {
        if (stopMovement) return;
        transform.position += direction * Vector3.right * Time.deltaTime * movementSpeed;
    }

    public void ChangeDirection()
    {
        direction *= -1;
    }

    private void PauseResume(bool gamePaused)
    {
        stopMovement = gamePaused;
    }

    public void ApproachingZombie()
    {
        audioManager.PlaySFX(AudioManager.SFX.Zombie);
    }

    public void LeavingZombie()
    {
        audioManager.FadeOutSFX();
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePaused -= PauseResume;
    }
    #endregion
}
