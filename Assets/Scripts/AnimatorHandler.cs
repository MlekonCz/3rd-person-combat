using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnimatorHandler : MonoBehaviour
    {
        private PlayerManager _playerManager;
        public Animator animator;
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private int vertical;
        private int horizontal;
        public bool canRotate;

        public void Initialize()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0f;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1f;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1f;
            }
            else
            {
                v = 0f;
            }
            #endregion
            #region Horizontal
            float h = 0f;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1f;
            }
            else
            {
                h = 0f;
            }
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            
            animator.SetFloat(vertical, v, 0.1f,Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f,Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);

        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (_playerManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0f;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0f;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}