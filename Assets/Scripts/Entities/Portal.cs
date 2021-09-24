using UnityEngine;

public class Portal : MonoBehaviour
{
    private AudioManager audioManager;
    // for managin portal audio
    private void Start()
    {
        audioManager = AudioManager.GetInstance;
    }

    public void AproachingPortal()
    {
        audioManager.PlaySFX(AudioManager.SFX.Portal);
    }

    public void LeavingPortal()
    {
        audioManager.FadeOutSFX();
    }
}
