using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip uiClick;
    public AudioClip magnetFlip;
    public AudioClip kamiExplode;
    public AudioClip playerDeath;
    public AudioClip bossSpawn;
    public AudioClip bossDeath;

    [Header("Music Tracks")]
    public AudioClip menuTheme;
    public AudioClip gameplayTheme;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayUIClick()
    {
        PlaySFX(uiClick, 0.7f);
    }

    public void PlayMagnetFlip()
    {
        PlaySFX(magnetFlip, 0.8f);
    }

    public void PlayKamiExplode()
    {
        PlaySFX(kamiExplode, 1f);
    }

    public void PlayPlayerDeath()
    {
        PlaySFX(playerDeath, 1f);
    }

    public void PlayBossSpawn()
    {
        PlaySFX(bossSpawn, 1f);
    }

    public void PlayBossDeath()
    {
        PlaySFX(bossDeath, 1f);
    }

    public void SetMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        if (musicSource.clip == menuTheme) return;
        musicSource.clip = menuTheme;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameplayMusic()
    {
        if (musicSource.clip == gameplayTheme) return;
        musicSource.clip = gameplayTheme;
        musicSource.loop = true;
        musicSource.Play();
    }

}
