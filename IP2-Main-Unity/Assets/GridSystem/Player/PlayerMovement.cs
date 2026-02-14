using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //new input system

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings\n")] 
    public float speed = 5f;

    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            Debug.Log("Move performed: " + moveInput);
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
    
    private void Update()
    {
        // Convert input to a movement vector on the XZ plane
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move relative to world axes (simple top-down)
        Vector3 displacement = inputDir * speed * Time.deltaTime;
        transform.position += displacement;
        
    }
}
