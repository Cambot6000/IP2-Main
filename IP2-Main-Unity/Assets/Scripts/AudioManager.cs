using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicSource;      //for background music
    public AudioSource sfxSource;        //for sound effects
    public AudioClip baseDamagedSound;
    public AudioClip rocketLaunchSound; 

    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Destroy(gameObject);
      }
        
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
       sfxSource.PlayOneShot(clip, volumeMultiplier * sfxVolume);
    }

    public void PlayRocketLaunch()
    {
     PlaySFX(rocketLaunchSound);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

        public void PlayBaseDamaged()
    {
        PlaySFX(baseDamagedSound);
    }

  
}
