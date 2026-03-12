using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundFXSlider;
    public TMP_Dropdown placingJoyStick;
    public TMP_Dropdown movingJoyStick;

    public void Start()
    {

        musicSlider.value = GameSettings.musicVolume;

        soundFXSlider.value = GameSettings.soundFXVolume;
    }


    public void ChangeMusicVolume()
    {
        GameSettings.musicVolume = musicSlider.value;
        
    }
    public void ChangeSoundFXVolume()
    {
        GameSettings.soundFXVolume = soundFXSlider.value;

    }
    public void ControllerForPlacing()
    {
     if(placingJoyStick.value == 0)
        {
            GameSettings.PlacingJoyStick = false;
        }
        else
        {
            GameSettings.PlacingJoyStick = true;
        }
    }
    public void ControllerForMoving()
    {
        if (movingJoyStick.value == 0)
        {
            GameSettings.MovingJoyStick = false;
        }
        else
        {
            GameSettings.MovingJoyStick = true;
        }
    }
}
