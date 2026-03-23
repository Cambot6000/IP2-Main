using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public static class GameSettings
{
    public enum Difficulty { Easy, Medium, Hard }
    public static Difficulty chosenDifficulty = Difficulty.Easy;

    public static float musicVolume = 0.5f;
    public static float soundFXVolume = 0.5f;

    public static bool PlacingJoyStick = true;
    public static bool MovingJoyStick = false;

}
