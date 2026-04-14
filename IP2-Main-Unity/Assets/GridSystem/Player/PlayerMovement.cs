using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; //new input system

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings\n")] 
    public float speed = 5f;

    private PlayerControls controls;
    private Vector2 moveInput;
    //private vector2 directionInput;
    [SerializeField] private Rigidbody rb;

    public static event Action<int> OnDoSomething;
    
    private bool canMove = true; //lock the player when ui is open, or any situation you want to stop the player from moving
    [SerializeField] private float verticalLock;

    [Header("Boundary Settings")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("Timer Related Stuff")]
    public SpriteRenderer playerIcon;
    public float timerDuration;
    public float currentTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            //directionInput = ctx.ReadValue<Vector2>();
            
            //Debug.Log("Move performed: " + moveInput);
        };
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        currentTime = timerDuration;
        UnTransparent(); //So that the wee guy doesn't disappear immediately
    }
    private void FixedUpdate()
    {
        //lock movement
        /*
        if (!canMove)
        {
            return; //if cant move return 
        }
        */

        // Convert input to a movement vector on the XZ plane
        //Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move relative to world axes 
        //Vector3 displacement = inputDir * speed * Time.deltaTime;
        //transform.position += displacement;
        //^ Old movement code incase my new stuff decides to break

        if (!canMove || moveInput.sqrMagnitude <= 0.01f)
        {
            if (currentTime > 0) //Timer to check if the player hasnt moved in a while and if they haveny they will fade out
            {
                currentTime -= Time.unscaledDeltaTime;
            }
            else
            {
                Color tempColour = playerIcon.color;
                tempColour.a = Mathf.MoveTowards(tempColour.a, 0f, Time.unscaledDeltaTime * 2f); //Slowly fades the player icon guy out
                playerIcon.color = tempColour;
            }
           
        }
        else
        {
            Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);
            currentTime = timerDuration;
            UnTransparent();
            Vector3 displacement = inputDir * speed * Time.fixedDeltaTime;
            Vector3 newPos = rb.position + displacement;

            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ); //Sticks the player within the bounds of the map instead of using colliders all around the place
            newPos.y = verticalLock;

            rb.MovePosition(newPos); //Move with the rigidbody so that the player doesn't get flung aboot anymore like they did for a while. Hopefully permanent fix to that issue
        }

        
        /*
        if (Mathf.Abs(transform.position.y - verticalLock) > 0.001f)
        {
            Vector3 fixedPos = transform.position;
            fixedPos.y = verticalLock;
            transform.position = fixedPos;
        }
        */
        //Rotation


    }
    
    //Check if the turret wheel is open, if it is lock movement
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    
    private void UnTransparent() //Makes the player appear again (sets transparency to 1)
    {
        Color tempColour = playerIcon.color;
        tempColour.a = 1f;
        playerIcon.color = tempColour;
    } 
}
