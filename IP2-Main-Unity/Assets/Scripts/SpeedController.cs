using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public Image[] speedButtons;
    public int buttonIndex;
    private float currentSpeed;
    public float targetSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedButtons[0].enabled = true;
        speedButtons[1].enabled = false;
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
                speedButtons[buttonIndex].enabled = false;
                buttonIndex = 1;
                speedButtons[buttonIndex].enabled = true;
                currentSpeed = targetSpeed;
                SpeedUp();
            }
            else if(buttonIndex == 1 && currentSpeed == targetSpeed)
            {
                speedButtons[buttonIndex].enabled = false;
                buttonIndex = 0;
                speedButtons[buttonIndex].enabled = true;
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
}
