using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerMenuMovement : MonoBehaviour
{
    private Vector2 inputDirection;
    public Button[] buttons;
    public int currentIndex = 0;

    //Cooldown variables
    private float moveTimer;
    public float moveDelay = 0.2f; //Time between moves in seconds

    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }

    public void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            //This triggers the OnClick() thing of the button
            buttons[currentIndex].onClick.Invoke();
            Color tempColor = buttons[currentIndex].image.color;
            tempColor.a = 0.85f;
            buttons[currentIndex].image.color = tempColor;
            print("Clicked: " + buttons[currentIndex].name);
        }
    }

    void Start()
    {
        UpdateButtonPos();
    }

    void Update()
    {
        //Reduce the timer over time
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }

        //Only move if the stick is pushed AND the timer is 0
        if (moveTimer <= 0 && Mathf.Abs(inputDirection.y) > 0.5f)
        {
            //Reset the cooldown timer
            moveTimer = moveDelay;

            //Move index of the array
            //Up is positive Y axis but arrays so you need to subtract, and the opposite for down because it is negative so you need to add in the array
            if (inputDirection.y > 0.5f) currentIndex--;
            else if (inputDirection.y < -0.5f) currentIndex++;

            //Loop the index
            if (currentIndex >= buttons.Length) currentIndex = 0;
            if (currentIndex < 0) currentIndex = buttons.Length - 1;

            UpdateButtonPos();
        }
    }

    void UpdateButtonPos()
    {
        foreach (Button btn in buttons)
        {
            //Gets the current colour of the button's image
            Color tempColor = btn.image.color;

            //If this button is the current selection, set the opacity to a hunner percent (1.0f)
            //If its not selected though, set it to 70% (0.7f) was 80 but 70 looks pretty good
            if (btn == buttons[currentIndex])
            {
                tempColor.a = 1.0f;
            }
            else
            {
                tempColor.a = 0.7f;
            }

            //This then the colour back
            btn.image.color = tempColor;
        }
    }
}