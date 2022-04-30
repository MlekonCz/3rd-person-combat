using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private Animator _animator;
    private CameraHandler cameraHandler;
    private PlayerLocomotion _playerLocomotion;
    
    
    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInteracting;
    
    private void Awake()
    {
        cameraHandler = CameraHandler.singleton;
    }
    
    private void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
        _animator = GetComponentInChildren<Animator>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        
        isInteracting = _animator.GetBool("isInteracting");
      
        
        
        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleRollingAndSprinting(delta);

    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta,_inputHandler.mouseX, _inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        _inputHandler.rollFlag = false;
        _inputHandler.sprintFlag = false;
        isSprinting = _inputHandler.b_Input;
        
        
    }
}
