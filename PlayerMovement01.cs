using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement01 : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;

    public float moveSpeed = 15f;
    public float jumpForce = 50f;
    private bool isJumping;

    private float moveVert = 0f;

    private void Start()
    {
        isJumping = false;
        jumpForce = 50f;
    }
    private void Awake()
    {
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 50f;

    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }
    private void Update()
    {
        moveVert = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        // left/right movement
        rb.velocity = moveVector * moveSpeed;

        //jumping
        //if not-jumping and moveVert is 1 
        //Added an extra jump number cus it's smoother I suppose... (11/08/23)
        //Originally 

        if (!isJumping && moveVert > 0.1f)
        {
            rb.AddForce(new Vector2(0f, moveVert * jumpForce + jumpForce), ForceMode2D.Impulse);
        }

    }

    //Ground collision stuff
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check for ground tag + onTrigger
        if(collision.gameObject.tag == "Platform")
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //check for ground tag + not-onTrigger
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }
    }


    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
