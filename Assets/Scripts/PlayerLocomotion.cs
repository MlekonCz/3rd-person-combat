using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Transform _cameraObject;
    private InputHandler _inputHandler;
    private Vector3 moveDirection;

    [HideInInspector] public Transform myTransform;
    [HideInInspector] public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")] 
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        _cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        _inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    #region Movement

    private Vector3 normalVector;
    private Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDirection = Vector3.zero;
        float moveOverride = _inputHandler.moveAmount;

        targetDirection = _cameraObject.forward * _inputHandler.vertical;
        targetDirection += _cameraObject.right * _inputHandler.horizontal;
        
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = myTransform.forward;
        }

        float rs = rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr,rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        moveDirection = _cameraObject.forward * _inputHandler.vertical;
        moveDirection += _cameraObject.right * _inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        float speed = movementSpeed;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        
        animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.animator.GetBool("isInteracting"))
        {
            return;
        }

        if (_inputHandler.rollFlag)
        {
            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;
            if (_inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("BackStep", true);
            }
        }
        
    }
    #endregion
}
