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
    private InputAction attackAction;
    private InputAction pauseMenuAction;
    private InputAction interactDialogueAction;

    private Vector2 moveDirection;

    public static event Action<Vector2> OnMoveInput;
    public static event Action OnAttackInput;
    public static event Action OnPauseGameInput;
    public static event Action OnInteractDialogue;


    private void Awake()
    {
        inputActions.Enable();
        var playerActionMap = inputActions.FindActionMap("PlayerInputs"); // var = comodin per variable que no conec


        moveAction = playerActionMap.FindAction("MoveDirection");
        attackAction = playerActionMap.FindAction("Attack");
        pauseMenuAction = playerActionMap.FindAction("PauseGame");
        interactDialogueAction = playerActionMap.FindAction("InteractDialogue");

        //  moveDirection = moveAction.ReadValue<Vector2>();
        moveAction.performed += MoveActionPerformed;
        attackAction.performed += AttackActionPerformed;
        pauseMenuAction.performed += PauseActionPerformed;
        interactDialogueAction.performed += DialogueActionPerformed;
    }

    void MoveActionPerformed(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    
    void AttackActionPerformed(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke(); // si es un button posar-ho dins del m�tode
    }
    
    void PauseActionPerformed(InputAction.CallbackContext context)
    {
        OnPauseGameInput?.Invoke(); // si es un button posar-ho dins del m�tode
    }
    
    void DialogueActionPerformed(InputAction.CallbackContext context)
    {
        OnInteractDialogue?.Invoke(); // si es un button posar-ho dins del m�tode
    }


    // Update is called once per frame
    void Update() // un invoke x cada valor que vulgui passar
    {
        OnMoveInput?.Invoke(moveDirection);
    }
}
