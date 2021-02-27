using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class CopyBoneToMotor : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private ConfigurableJoint joint;

        [SerializeField]
        private Transform reference;

        [SerializeField]
        private bool copyRotation = true;

        [SerializeField]
        private bool copyPosition = true;

        #endregion

        #region Properties

        public ConfigurableJoint Joint { get => joint; set => joint = value; }
        public Transform Reference { get => reference; set => reference = value; }

        #endregion

        private void FixedUpdate()
        {
            if (copyPosition)
            {
                joint.targetPosition = reference.localPosition;
            }

            if (copyRotation)
            {
                joint.targetRotation = Quaternion.Inverse(reference.localRotation);
            }
        }
    }
}
