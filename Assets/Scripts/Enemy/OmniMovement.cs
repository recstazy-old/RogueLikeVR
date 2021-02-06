using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class OmniMovement : MonoBehaviour, IPhysicsMovement
    {
        #region Fields

        [SerializeField]
        private float forceMagnitude;

        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        private float forceFallofDistance;

        [SerializeField]
        private float forceApplyHeight;

        #endregion

        #region Properties

        public float Force { get => forceMagnitude; set => forceMagnitude = value; }
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

        #endregion

        public void MoveBody(Rigidbody body, Transform currentTarget)
        {
            var xzVelocity = body.velocity;
            xzVelocity.y = 0f;

            if (xzVelocity.magnitude <= maxSpeed)
            {
                Vector3 force = (currentTarget.transform.position - body.transform.position).normalized;
                force.y = 0f;

                var distance = currentTarget.position - body.position;
                distance.y = 0f;
                float alpha = Mathf.Clamp01(distance.magnitude - forceFallofDistance);
                alpha *= alpha;
                float magnetude = Mathf.Lerp(0f, forceMagnitude, alpha);
                force = force * magnetude;

                body.AddForceAtPosition(force, body.position + Vector3.up * forceApplyHeight, ForceMode.Force);
            }
        }
    }
}
