using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public Image[] speedButtons;
    public int buttonIndex;
    private float currentSpeed;
    public float targetSpeed;
    public float fadeDuration = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetImageTransparency(speedButtons[0], 1f);
        SetImageTransparency(speedButtons[1], 0f);
        //speedButtons[0].enabled = true;
        //speedButtons[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputValue value)
    {
        //Debug.Log("Keep up");
        if (value.isPressed)
        {
            //Debug.Log("speed me up");
            if (buttonIndex == 0 && currentSpeed < targetSpeed) 
            {
                //speedButtons[buttonIndex].enabled = false;
                StartCoroutine(FadeButton(speedButtons[buttonIndex], 0f));
                buttonIndex = 1;
                //speedButtons[buttonIndex].enabled = true;
                StartCoroutine(FadeButton(speedButtons[buttonIndex], 1f));
                currentSpeed = targetSpeed;
                SpeedUp();
            }
            else if(buttonIndex == 1 && currentSpeed == targetSpeed)
            {
                //speedButtons[buttonIndex].enabled = false;
                StartCoroutine(FadeButton(speedButtons[buttonIndex], 0f));
                buttonIndex = 0;
                //speedButtons[buttonIndex].enabled = true;
                StartCoroutine(FadeButton(speedButtons[buttonIndex], 1f));
                currentSpeed = 1f;
                SlowDown();
            }
        }
    }

    void SpeedUp()
    {
        Time.timeScale = targetSpeed;
    }

    void SlowDown()
    {
        Time.timeScale = 1f;
    }

    IEnumerator FadeButton(Image speedyDos, float targetTransparency)
    {
        float startTransparency = speedyDos.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime; //Use unscaledDeltaTime because Time.timeScale changes becasue of button hits
            float newAlpha = Mathf.Lerp(startTransparency, targetTransparency, time / fadeDuration);
            SetImageTransparency(speedyDos, newAlpha);
            yield return null;
        }
        SetImageTransparency(speedyDos, targetTransparency);
    }
    void SetImageTransparency(Image speedy, float transparency)
    {
        Color colour = speedy.color;
        colour.a = transparency;
        speedy.color = colour;
    }
}
