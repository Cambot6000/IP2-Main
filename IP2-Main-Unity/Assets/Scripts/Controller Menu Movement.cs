using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerMenuMovement : MonoBehaviour
{
    private Vector2 inputDirection;
    public Button[] buttons;
    public int currentIndex = 0;
    public float fadeNumber;

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

    //Reduce the timer over time
    void Update()
    {
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }

        //It's a timer
        if (moveTimer <= 0)
        {
            //Checks to see if X is pushed Right OR Y is pushed Up
            //Both of these will progress through what buttons are there
            if (inputDirection.x > 0.5f || inputDirection.y < -0.5f)
            {
                moveTimer = moveDelay;
                currentIndex++;

                if (currentIndex >= buttons.Length) currentIndex = 0;
                UpdateButtonPos();
            }
            //Checks to see if X is pushed Left OR Y is pushed Down
            //This does the opposite of the last section of commentary explains (it goes backwards or down)
            //Since buttons are currently laid out in a kind of diagonal line it makes more sense to make
            //the down one progress button by button rather than go backwards if you understand what I'm trying to say
            else if (inputDirection.x < -0.5f || inputDirection.y > 0.5f)
            {
                moveTimer = moveDelay;
                currentIndex--;

                if (currentIndex < 0) currentIndex = buttons.Length - 1;
                UpdateButtonPos();
            }
        }
    }

    void UpdateButtonPos()
    {
        foreach (Button btn in buttons)
        {
            //Gets the current colour of the button's image
            Color tempColor = btn.image.color;

            //If this button is the current selection, set the opacity to a hunner percent (1.0f)
            if (btn == buttons[currentIndex])
            {
                tempColor.a = 1.0f;
            }
            else
            {
                if(fadeNumber > 0.9f || fadeNumber < 0.5f)
                {
                    fadeNumber = 0.65f;
                }
                tempColor.a = fadeNumber;
            }

            //This then the colour back
            btn.image.color = tempColor;
        }
    }
}