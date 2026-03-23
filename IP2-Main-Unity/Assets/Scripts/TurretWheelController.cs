// Foundation and concept created by Sam, implemented by Callum //last edited 25/02/2026
//Comments to be done, just pushed this out to make sure a prototype was done

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Object = System.Object; // for controller/keyboard support 

public class TurretWheelController : MonoBehaviour
{
    //public Animator anim;
    //private bool turretWheelSelected = false;
    
    [Header("Ui Settings")]
    public Image selectedItem;
    public Sprite noImage;
    
    public static int turretID;
    public GameObject wheelRoot;
    private bool isOpen;

    [Header("Buttons (in circular order)")]
    public TurretWheelButtonController[] buttons; // used for right-stick selection
    
    //index to store the current button hovered over, for controller support i think there is an easier way to do this through animation trees but i dont really know much about that, change if you think there is a better way
    private int hoverIndex = -1;
        

    private void Start()    
    {
        SetOpen(false);
        Debug.Log("! Debug -> Game started, debug wheel set to CLOSED");    
    }

    void Update()
    {
        Debug.LogWarning("TurretWheelController Update running");

        // keyboard support
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            Debug.Log("! Debug -> Tab has been pressed");
            SetOpen(!isOpen);
        }

        // controller support: hold left bumper to open, release to close
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftShoulder.wasPressedThisFrame)
                SetOpen(true);

            if (Gamepad.current.leftShoulder.wasReleasedThisFrame)
                SetOpen(false);
        }

        // if wheel is closed, just reflect current selection
        if (!isOpen)
        {

            //if you want to add effects when something is clicked do it here, the turret selectio is stored in my building scriot
            switch (turretID)
            {
                case 0: // nothing selected
                    selectedItem.sprite = noImage;
                    break;
                case 1:
                    Debug.Log("Turret 1 selected");
                    break;
                case 2:
                    Debug.Log("Turret 2 selected");
                    break;
                case 3:
                    Debug.Log("Turret 3 selected");
                    break;
                case 4:
                    Debug.Log("Turret 4 selected");
                    break;
                case 5:
                    Debug.Log("Turret 5 selected");
                    break;
            }

            return;
        }

        // controller RIGHT stick to select buttons while wheel is open, 12 oclock = stick up, thats how i think about it
        // controller RIGHT stick to select buttons while wheel is open, 12 oclock = stick up, thats how i think about it
        if (Gamepad.current != null && buttons != null && buttons.Length > 0)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();
            Debug.Log("Stick: " + stick); // debug so we can see raw values

            // When stick is near the centre, clear hover and do nothing else.
            if (stick.sqrMagnitude <= 0.25f)
            {
                if (hoverIndex >= 0)
                {
                    buttons[hoverIndex].HoverExit();
                    hoverIndex = -1;
                }
            }
            else
            {
                // base angle in degrees, 0 at +X (right), CCW positive
                float rawAngle = Mathf.Atan2(stick.y, -stick.x) * Mathf.Rad2Deg;

                // rotate so 0 is UP and clockwise is positive (like a clock)
                // up (0,1) -> rawAngle = 90 -> angleClock = 0
                float angleClock = 90f - rawAngle;  
                if (angleClock < 0f) angleClock += 360f;
                if (angleClock >= 360f) angleClock -= 360f;

                int segmentCount = buttons.Length;
                float segmentSize = 360f / segmentCount;

                // center each segment around its direction
                float centeredAngle = angleClock + segmentSize * 0.5f;
                if (centeredAngle >= 360f) centeredAngle -= 360f;

                int index = Mathf.FloorToInt(centeredAngle / segmentSize);
                index = Mathf.Clamp(index, 0, segmentCount - 1);

                Debug.Log($"angleClock={angleClock}, centered={centeredAngle}, index={index}"); //debug stuff for my sanity,

                // Only change hover when the index changes
                if (index != hoverIndex)
                {
                    // remove hover effect
                    if (hoverIndex >= 0)
                        buttons[hoverIndex].HoverExit();

                    // apply hover effect
                    hoverIndex = index;
                    buttons[hoverIndex].HoverEnter();
                }
            }

            // confirm selection with A / Cross (south button)
            bool confirm = Gamepad.current.buttonSouth.wasPressedThisFrame;

            if (confirm && hoverIndex >= 0)
            {
                buttons[hoverIndex].Selected();
            }
        }

    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        if (wheelRoot != null)
            wheelRoot.SetActive(open); // show/hide the wheel UI

        // Tell Building to pause/resume build input while wheel is open
        if (Building.current != null)
            Building.current.SetWheelOpen(open);
        
        
        //lock player movement
        PlayerMovement pm = FindFirstObjectByType<PlayerMovement>();
        if (pm != null)                                         
            pm.SetCanMove(!open);    
    }
}
