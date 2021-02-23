using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.AI
{
    public class StandingLocomotion : MonoBehaviour, IPhysicsMovement, IPhysicsLookAt
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

        private Transform lookTarget;
        private Rigidbody body;

        #endregion

        #region Properties

        public bool IsActive { get; set; }

        #endregion

        private void FixedUpdate()
        {
            if (IsActive)
            {
                if (lookTarget != null)
                {
                    var distance = lookTarget.position - body.position;
                    distance.y = 0f;
                    var lookRotation = Quaternion.LookRotation(distance.normalized, Vector3.up);
                    Quaternion newRotation = Quaternion.Lerp(body.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime);
                    body.MoveRotation(newRotation);
                }
            }
        }

        public void MoveBody(Rigidbody body, Transform currentTarget)
        {
            if (IsActive)
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
            }
        }

        public void MoveBodyRotation(Rigidbody body, Transform target)
        {
            this.body = body;
            lookTarget = target;
        }
    }
}
