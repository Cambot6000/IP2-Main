using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioClip menuMusic;

    void Start()
    {
        AudioManager.Instance.PlayMusic(menuMusic);
    }
}