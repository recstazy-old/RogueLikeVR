using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class SetCustomTargetValues : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private bool usePosition = false;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private bool useRotation = true;

        private ConfigurableJoint joint;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            joint = GetComponent<ConfigurableJoint>();
        }

        private void FixedUpdate()
        {
            if (joint != null)
            {
                if (useRotation)
                {
                    joint.targetRotation = Quaternion.Inverse(Quaternion.Euler(rotation));
                }
                
                if (usePosition)
                {
                    joint.targetPosition = position;
                }
            }
        }
    }
}
