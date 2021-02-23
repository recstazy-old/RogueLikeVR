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

        #endregion

        #region Properties

        #endregion

        private void Update()
        {
            if (body != null && animator != null)
            {
                var velocityLocal = body.transform.InverseTransformVector(body.velocity);
                animator.SetFloat(directionXName, velocityLocal.x);
                animator.SetFloat(directionZName, velocityLocal.z);
            }
        }
    }
}
