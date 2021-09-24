using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    #region Enum
    public enum BackgroundMusic
    {
        HumanMusic,
        GhostMusic,
        MainMenuMusic,
        GameOver
    }
    public enum CharacterSFX
    {
        Jump,
        Walking,
        Death,
        GhostFloating
    }
    public enum SFX
    {
        BridgeStart,
        BridgeStop,
        Zombie,
        PickUp,
        Portal
    }
    #endregion
    #region Variables
    #region Audio
    public AudioSource characterSFXSource;
    public AudioClip[] CharacterSFXClips;
    public AudioSource musicSource;
    public AudioClip[] musicClips;
    public AudioSource SFXSource;
    public AudioClip[] SFXClips;
    public AudioMixer mixer;
    float volumeRange = 0.1f;
    float pitchRange = 0.2f;
    float initialVolume;
    #endregion
    private static AudioManager instance;
    #endregion

    #region Methods
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // when starting it will awlays play this music
        // because you will always start with the human player
        mixer.GetFloat("SFXVolume", out initialVolume);
    }

    /// <summary>
    /// checks from a list to play music from
    /// </summary>
    public void PlayMusic(BackgroundMusic musicClip)
    {
        switch (musicClip)
        {
            case BackgroundMusic.HumanMusic:
                musicSource.clip = musicClips[(int)BackgroundMusic.HumanMusic];
                break;
            case BackgroundMusic.GhostMusic:
                musicSource.clip = musicClips[(int)BackgroundMusic.GhostMusic];
                break;
            case BackgroundMusic.MainMenuMusic:
                musicSource.clip = musicClips[(int)BackgroundMusic.MainMenuMusic];
                break;
            case BackgroundMusic.GameOver:
                musicSource.clip = musicClips[(int)BackgroundMusic.GameOver];
                break;
        }

        musicSource.Play();
    }

    /// <summary>
    /// fades out current music and plays the next one
    /// </summary>
    public void FadeMusic(BackgroundMusic musicClip)
    {
        StopAllCoroutines(); // stops fade in/out, it helps when spamming this
        StartCoroutine(FadeOut(musicClip));
    }

    /// <summary>
    /// smoothly lowers music 
    /// </summary>
    IEnumerator FadeOut(BackgroundMusic musicClip)
    {
        float duration = 0.01f;
        float lowerStep = 2f;
        int lowestVolume = -34;

        float musicVolume;
        mixer.GetFloat("MusicVolume", out musicVolume);

        while (musicVolume > lowestVolume)
        {
            mixer.SetFloat("MusicVolume", musicVolume -= lowerStep);
            yield return new WaitForSeconds(duration);
        }

        PlayMusic(musicClip);
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// smoothly turns music up
    /// </summary>
    IEnumerator FadeIn()
    {
        float duration = 0.05f;
        float upStep = 1f;
        int highestVolume = -28;

        float musicVolume;
        mixer.GetFloat("MusicVolume", out musicVolume);

        while (musicVolume < highestVolume)
        {
            mixer.SetFloat("MusicVolume", musicVolume += upStep);
            yield return new WaitForSeconds(duration);
        }
    }

    /// <summary>
    /// plays a sfx 
    /// </summary>
    public void PlayCharSFX(CharacterSFX sfxClip)
    {
        switch (sfxClip)
        {
            case CharacterSFX.Jump:
                characterSFXSource.clip = CharacterSFXClips[(int)CharacterSFX.Jump];
                break;
            case CharacterSFX.Death:
                characterSFXSource.clip = CharacterSFXClips[(int)CharacterSFX.Death];
                break;
        }
        float startingVolume = 0.5f;
        float startingPitch = 0.9f;
        RandomizeSound(startingVolume, startingPitch);
    }

    /// <summary>
    /// checks if this is source is not playing
    /// and plays the walking sound
    /// </summary>
    public void WalkingSFX(CharacterSFX sfxClip)
    {
        if (!characterSFXSource.isPlaying)
        {
            float startingVolume = 0.4f;
            float startingPitch = 0.9f;
            switch (sfxClip)
            {
                case CharacterSFX.Walking:
                    characterSFXSource.clip = CharacterSFXClips[(int)CharacterSFX.Walking];
                    break;
                case CharacterSFX.GhostFloating:
                    characterSFXSource.clip = CharacterSFXClips[(int)CharacterSFX.GhostFloating];
                    break;
            }

            RandomizeSound(startingVolume, startingPitch);
        }
    }

    /// <summary>
    /// Plays sounds on SFX Source
    /// </summary>
    public void PlaySFX(SFX SFXClip)
    {
        switch (SFXClip)
        {
            case SFX.BridgeStart:
                SFXSource.clip = SFXClips[(int)SFX.BridgeStart];
                break;
            case SFX.BridgeStop:
                SFXSource.clip = SFXClips[(int)SFX.BridgeStop];
                break;
            case SFX.Zombie:
                SFXSource.clip = SFXClips[(int)SFX.Zombie];
                float zombieVol = -10; // zombie sfx es re fuerte, aca lo bajamos un poco
                mixer.SetFloat("SFXVolume", zombieVol);
                SFXSource.loop = true;
                break;
            case SFX.PickUp:
                SFXSource.clip = SFXClips[(int)SFX.PickUp];
                break;
            case SFX.Portal:
                SFXSource.clip = SFXClips[(int)SFX.Portal];
                float portalVol = -15; // portal sfx es re fuerte, aca lo bajamos un poco
                mixer.SetFloat("SFXVolume", portalVol);
                SFXSource.loop = true;
                // esta repetido con lo de arriba pero por poco tiempo se queda asi
                break;
        }
        SFXSource.Play();
    }

    /// <summary>
    /// Fade out for sfx
    /// </summary>
    public void FadeOutSFX()
    {
        StopAllCoroutines();
        StartCoroutine(FadingOutSFX());
    }

    /// <summary>
    /// coroutine for fadeoutsfx
    /// and stops looping
    /// </summary>
    IEnumerator FadingOutSFX()
    {
        float duration = 0.01f;
        float lowerStep = 2f;
        int lowestVolume = -30;

        float sfxVolume;
        mixer.GetFloat("SFXVolume", out sfxVolume);

        while (sfxVolume > lowestVolume)
        {
            mixer.SetFloat("SFXVolume", sfxVolume -= lowerStep);
            yield return new WaitForSeconds(duration);
        }
        //resets sfx parameters
        SFXSource.loop = false;
        mixer.SetFloat("SFXVolume", initialVolume);
        SFXSource.Pause();
    }

    /// <summary>
    /// varies volume and pitch of a sound to make it
    /// less monotonous
    /// </summary>
    void RandomizeSound(float volume, float pitch)
    {
        characterSFXSource.volume = GetRandom(volume, volumeRange);
        characterSFXSource.pitch = GetRandom(pitch, pitchRange);
        characterSFXSource.Play();
    }

    /// <summary>
    /// Random.Range a little smaller to make it easier to read
    /// </summary>
    float GetRandom(float value, float range)
    {
        return Random.Range(value - range, value + range);
    }

    public static AudioManager GetInstance
    {
        get { return instance; }
    }
    #endregion
}
