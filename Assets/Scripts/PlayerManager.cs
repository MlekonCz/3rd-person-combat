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
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    
    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
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
        canDoCombo = _animator.GetBool("canDoCombo");
        
        
        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleRollingAndSprinting(delta);
        _playerLocomotion.HandleFalling(delta,_playerLocomotion.moveDirection);

        CheckForInteractableObject();

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
        _inputHandler.rb_Input = false;
        _inputHandler.rt_Input = false;
        _inputHandler.dPadRight = false;
        _inputHandler.dPadLeft = false;
        _inputHandler.dPadDown = false;
        _inputHandler.dPadUp = false;
        _inputHandler.a_Input = false; 

        if (isInAir)
        {
            _playerLocomotion.inAirTimer += Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position,0.3f, transform.forward,out hit,1f,cameraHandler.ignoreLayers))
        {
            if (hit.collider.CompareTag(TagManager.Interactable))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText; 
                    //Set Ui text to the interactable objects text

                    if (_inputHandler.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
    }
    
    
}
