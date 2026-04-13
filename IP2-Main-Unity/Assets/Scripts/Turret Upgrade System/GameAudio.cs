using UnityEngine;

public class GameStartAudio : MonoBehaviour
{
    public AudioClip gameMusic;
    void Start()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayRocketLaunch();
        AudioManager.Instance.SetMusicVolume(0.2f);     
        AudioManager.Instance.PlayMusic(gameMusic); 
    }
}