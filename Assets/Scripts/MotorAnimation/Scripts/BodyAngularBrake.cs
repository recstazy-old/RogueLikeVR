using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class BodyAngularBrake : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Vector3 axis;

        [SerializeField]
        private float brakeAmount;

        private Rigidbody body;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (body != null)
            {
                axis.x = Mathf.Clamp01(axis.x);
                axis.y = Mathf.Clamp01(axis.y);
                axis.z = Mathf.Clamp01(axis.z);

                var velocity = body.angularVelocity;
                velocity.x = Mathf.Lerp(velocity.x, 0f, axis.x * brakeAmount * Time.fixedDeltaTime);
                velocity.y = Mathf.Lerp(velocity.y, 0f, axis.y * brakeAmount * Time.fixedDeltaTime);
                velocity.z = Mathf.Lerp(velocity.z, 0f, axis.z * brakeAmount * Time.fixedDeltaTime);

                body.angularVelocity = velocity;
            }
        }
    }
}
