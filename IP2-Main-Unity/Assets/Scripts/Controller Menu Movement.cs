using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ControllerMenuMovement : MonoBehaviour
{
    private Vector2 inputDirection;
    public Selectable[] UIElements;
    public int currentIndex = 0;
    public float fadeNumber;
    public bool sliderSelected;
    public bool dropdownSelected;

    //Cooldown variables
    private float moveTimer;
    public float moveDelay = 0.2f; //Time between moves in seconds

    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }

    //If you hit X or A you can do this bit, or Enter too
    public void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            if (UIElements[currentIndex] is Button buttonVar)
            {
                //This triggers the OnClick() thing of the button
                buttonVar.onClick.Invoke();
                Color tempColor = UIElements[currentIndex].image.color;
                tempColor.a = 0.85f;
                UIElements[currentIndex].image.color = tempColor;
                print("Clicked: " + UIElements[currentIndex].name);
            }
            else if (UIElements[currentIndex] is TMP_Dropdown dropdownMenu)
            {
                dropdownMenu.Show();
                dropdownSelected = true;
            }
            else if (UIElements[currentIndex] is Slider slidy && sliderSelected == false)
            {
                sliderSelected = true;
            }
        }
    }

    //This is like hitting on submit but the evil version where it deselects it
    //Activated with circle or B, I think
    public void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            sliderSelected = false;
            dropdownSelected = false;
            if (UIElements[currentIndex] is TMP_Dropdown dropdownMenu)
            {
                dropdownMenu.Hide();
            }
            print("Cleared navigation the now");
            
        }
    }

    void Start()
    {
        UpdateButtonPos();
        sliderSelected = false;
    }

    void Update()
    {
        //Reduce the timer over time because it's a timer
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }

        if (moveTimer <= 0)
        {
            //Do Slider Movement when it's selected by hitting X or A or whatever button works, I think enter would work on a keyboard
            if (sliderSelected && UIElements[currentIndex] is Slider slidy)
            {
                if (Mathf.Abs(inputDirection.x) > 0.5f)
                {
                    float step = (slidy.maxValue - slidy.minValue) * 0.1f;
                    if (inputDirection.x > 0) slidy.value += step;
                    else slidy.value -= step;

                    moveTimer = 0.1f; //Faster movement speed timer for sliders because it's funny
                }
            }

            else if (dropdownSelected && UIElements[currentIndex] is TMP_Dropdown dropdownDos)
            {
                //Move the selection thingy forwards
                if (inputDirection.x > 0.5f)
                {
                    dropdownDos.value++;
                    if (dropdownDos.value >= dropdownDos.options.Count) dropdownDos.value = 0; // Loop to start
                    moveTimer = moveDelay;
                }
                //Move selection backwards
                else if (inputDirection.x < -0.5f)
                {
                    dropdownDos.value--;
                    if (dropdownDos.value < 0) dropdownDos.value = dropdownDos.options.Count - 1; // Loop to end
                    moveTimer = moveDelay;
                }
            }
            //Older Menu Navigation bit
            else if (!sliderSelected && !dropdownSelected)
            {
                //Checks to see if X is pushed Right OR Y is pushed Up
                //Both of these will progress through what buttons are there
                if (inputDirection.x > 0.5f || inputDirection.y < -0.5f)
                {
                    currentIndex++;
                    if (currentIndex >= UIElements.Length) currentIndex = 0;

                    moveTimer = moveDelay;
                    UpdateButtonPos();
                }
                //Checks to see if X is pushed Left OR Y is pushed Down
                //This does the opposite of the last section of commentary explains (it goes backwards or down)
                //Since buttons are currently laid out in a kind of diagonal line it makes more sense to make
                //the down one progress button by button rather than go backwards if you understand what I'm trying to say
                else if (inputDirection.x < -0.5f || inputDirection.y > 0.5f)
                {
                    currentIndex--;
                    if (currentIndex < 0) currentIndex = UIElements.Length - 1;

                    moveTimer = moveDelay;
                    UpdateButtonPos();
                }
            }
        }
    }

    void UpdateButtonPos()
    {
        foreach (Selectable UIThing in UIElements)
        {
            //Gets the current colour of the button's image
            //No longer just buttons though is it
            //It's a bit of everything now but the method is still called UpdateButtonPos() because that is what it was called before all the
            //extra stuff was added in
            Color tempColor = UIThing.image.color;

            //If this button (OR other UI element like I said earlier about not just buttons anymore) is the current selection,
            //set the opacity to a hunner percent (1.0f)
            if (UIThing == UIElements[currentIndex])
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

            //This then the colour back if you get me
            UIThing.image.color = tempColor;
        }
    }
}