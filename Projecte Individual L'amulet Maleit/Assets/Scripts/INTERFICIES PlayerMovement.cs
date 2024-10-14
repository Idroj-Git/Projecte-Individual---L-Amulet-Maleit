using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class INTERFICIES : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 10f;

    private bool isPaused;
    private bool isGrounded;

    private Rigidbody2D rb;

    public Transform groundCheck;

    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector2 movementInput;

    float lastInput = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        inputActions.Enable();
        var playerActionMap = inputActions.FindActionMap("Movement"); // var = comodin per variable que no conec
        moveAction = playerActionMap.FindAction("Horizontal");
        jumpAction = playerActionMap.FindAction("Jump");

        moveAction.performed += MoveActionPerformed;

    }


    void MoveActionPerformed(InputAction.CallbackContext context)
    {
        lastInput = context.ReadValue<float>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            //if (moveAction.WasReleasedThisFrame()) INNECESSARI PERQUÈ VA AMB UN PASS THROUGH, I NO UN VALUE (el pass through retorna tots els canvis, no només quan s'activa un dels components)
            //{
            //    lastInput = 0;
            //}
            rb.velocity = new Vector2(lastInput * moveSpeed, rb.velocity.y);

            if (jumpAction.WasPerformedThisFrame() && isGrounded)
            {
                Jump();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }
}
