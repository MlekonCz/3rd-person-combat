using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] public float horizontal;
    [SerializeField] public float vertical;
    [SerializeField] public float moveAmount;
    [SerializeField] public float mouseX;
    [SerializeField] public float mouseY;

    public bool b_Input;
    public bool rb_Input;
    public bool rt_Input;

    public bool sprintFlag;
    public bool rollFlag;
    public float rollInputTimer;
    

    private PlayerControls _inputActions;
    private PlayerAttacker _playerAttacker;
    private PlayerInventory _playerInventory;
   

    private Vector2 _movementInput;
    private Vector2 _cameraInput;


    private void Awake()
    {
        _playerAttacker = GetComponent<PlayerAttacker>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    public void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerControls();
            _inputActions.PlayerMovement.Movement.performed += movementInputActions
                => _movementInput = movementInputActions.ReadValue<Vector2>();
            _inputActions.PlayerMovement.Camera.performed += cameraInputActions => _cameraInput = cameraInputActions.ReadValue<Vector2>();
        }
        _inputActions.Enable();
    }

    public void OnDisable()
    {
        _inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollingInput(delta);
        HandleAttackInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = _movementInput.x;
        vertical = _movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = _cameraInput.x;
        mouseY = _cameraInput.y;

    }

    private void HandleRollingInput(float delta)
    {
        b_Input = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed; 
        if (b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0f;
        }
    }

    private void HandleAttackInput(float delta)
    {
        _inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        _inputActions.PlayerActions.RT.performed += i => rt_Input = true;

        //RB Input handles the RIGHT hand weapon's light attack
        if (rb_Input)
        {
            _playerAttacker.HandleLightAttack(_playerInventory.rightWeapon);
        }

        //RB Input handles the RIGHT hand weapon's heavy attack

        if (rt_Input)
        {
            _playerAttacker.HandleHeavyAttack(_playerInventory.rightWeapon);
        }
        
    }
    
}
