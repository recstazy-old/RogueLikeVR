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
        private float movementTreshold;

        [SerializeField]
        private string isMovingName;

        private Vector3? lastPosition;
        private Vector3? velocity;

        #endregion

        #region Properties

        #endregion

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
