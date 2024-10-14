using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    private InputAction moveAction;

    private Vector2 moveDirection;

    public static event Action<Vector2> OnMoveInput;


    private void Awake()
    {
        inputActions.Enable();
        var playerActionMap = inputActions.FindActionMap("PlayerInputs"); // var = comodin per variable que no conec
        moveAction = playerActionMap.FindAction("MoveDirection");
      //  moveDirection = moveAction.ReadValue<Vector2>();
        moveAction.performed += MoveActionPerformed;
    }

    void MoveActionPerformed(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }


    // Update is called once per frame
    void Update() // un invoke x cada valor que vulgui passar
    {
        OnMoveInput?.Invoke(moveDirection);
    }
}
