using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class StandingLocomotion : MonoBehaviour, IPhysicsMovement
    {
        #region Fields

        [SerializeField]
        private float accelerationForce = 1000f;

        [SerializeField]
        private float maxForce = 1000f;

        [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField]
        private float stopDistance;

        [SerializeField]
        private float stopVelocity = 0.01f;

        #endregion

        #region Properties

        #endregion

        public void MoveBody(Rigidbody body, Transform currentTarget)
        {
            var distance = currentTarget.position - body.position;

            if (distance.magnitude > stopDistance)
            {
                var force = distance * accelerationForce;
                force = Mathf.Min(force.magnitude, maxForce) * force.normalized;
                body.AddForce(force, ForceMode.Force);
            }
            else
            {
                if (body.velocity.magnitude >= stopVelocity)
                {
                    body.velocity -= body.velocity * 0.1f;
                }
            }
            
            Quaternion newRotation = Quaternion.Lerp(body.rotation, currentTarget.rotation, rotationSpeed * Time.fixedDeltaTime);
            body.MoveRotation(newRotation);
        }
    }
}
