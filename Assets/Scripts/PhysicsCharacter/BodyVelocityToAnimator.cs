using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
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
                animator.SetFloat(directionXName, body.velocity.x);
                animator.SetFloat(directionZName, body.velocity.z);
            }
        }
    }
}
