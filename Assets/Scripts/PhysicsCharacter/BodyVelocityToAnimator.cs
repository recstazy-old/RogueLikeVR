using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.PhysicsCharacters
{
    public class BodyVelocityToAnimator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private string directionXName;

        [SerializeField]
        private string directionZName;

        [SerializeField]
        private string isMovingName;

        [SerializeField]
        private float movementTreshold;

        private Vector3? lastPosition;
        private Vector3? velocity;

        #endregion

        #region Properties

        public Animator Animator { get => animator; set => animator = value; }
        public Rigidbody Body { get => body; set => body = value; }
        public string DirectionXName { get => directionXName; set => directionXName = value; }
        public string DirectionZName { get => directionZName; set => directionZName = value; }
        public string IsMovingName { get => isMovingName; set => isMovingName = value; }
        public float MovementTreshold { get => movementTreshold; set => movementTreshold = value; }

        #endregion

        private void Reset()
        {
            directionXName = "DirectionX";
            directionZName = "DirectionZ";
            isMovingName = "IsMoving";
            movementTreshold = 0.5f;
        }

        private void LateUpdate()
        {
            if (lastPosition != null)
            {
                velocity = (transform.position - lastPosition) / Time.deltaTime;
            }

            if (velocity != null && body != null && animator != null)
            {
                var velocityLocal = body.transform.InverseTransformVector(body.velocity);

                animator.SetBool(isMovingName, velocityLocal.magnitude >= movementTreshold);
                animator.SetFloat(directionXName, velocityLocal.x);
                animator.SetFloat(directionZName, velocityLocal.z);
            }

            lastPosition = transform.position;
        }
    }
}
