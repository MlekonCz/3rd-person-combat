using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Transform _cameraObject;
    private InputHandler _inputHandler;
    public Vector3 moveDirection;
    private PlayerManager _playerManager;

    [HideInInspector] public Transform myTransform;
    [HideInInspector] public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Ground & Air Detection Stats")]
    [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] private float minimumDistanceToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = 0.4f;
    private LayerMask ignoreForGroundCheck;
    public float inAirTimer;
    
    
    [Header("Movement Stats")] 
    [SerializeField] private float _walkingSpeed = 1f;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _fallingSpeed = 110f;

    
    
    
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        _cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        _playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
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

        float rs = _rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr,rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        if (_inputHandler.rollFlag){return;}
        if (_playerManager.isInteracting){return;}
        
        
        moveDirection = _cameraObject.forward * _inputHandler.vertical;
        moveDirection += _cameraObject.right * _inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        float speed = _movementSpeed;

        if (_inputHandler.sprintFlag && _inputHandler.moveAmount >0.5f)
        {
            speed = _sprintSpeed;
            _playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (_inputHandler.moveAmount <0.5f)
            {
                moveDirection *= _walkingSpeed;
                _playerManager.isSprinting = false;

            }
            else
            {
                _playerManager.isSprinting = false;
                moveDirection *= speed;
            }
            
        }
       

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        
        animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0, _playerManager.isSprinting);

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

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        _playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin,myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (_playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * _fallingSpeed);
            rigidbody.AddForce(moveDirection * _fallingSpeed / 5f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceToBeginFall, Color.red,0.1f,false);
        
        if (Physics.Raycast(origin,-Vector3.up, out hit, minimumDistanceToBeginFall,ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            _playerManager.isGrounded = true;
            targetPosition.y = tp.y;
            if (_playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in air for " + inAirTimer);
                    animatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0f;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0f;
                }

                _playerManager.isInAir = false;
            }
        }
        else
        {
            if (_playerManager.isGrounded)
            {
                _playerManager.isGrounded = false;
            }

            if (_playerManager.isInAir == false)
            {
                if (_playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (_movementSpeed / 2);
                _playerManager.isInAir = true;
            }
        }
        
        if (_playerManager.isInteracting || _inputHandler.moveAmount > 0)
        { 
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        { 
            myTransform.position = targetPosition;
        }
            
    }

    #endregion
}
