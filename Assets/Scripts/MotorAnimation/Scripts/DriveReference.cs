using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class DriveReference : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private ConfigurableJoint joint;

        [SerializeField]
        private Transform reference;

        [SerializeField]
        private bool copyRotation = true;

        [SerializeField]
        private bool copyPosition = false;

        private Vector3 startPosition;
        private Quaternion startRotation;

        #endregion

        #region Properties

        public ConfigurableJoint Joint { get => joint; set => joint = value; }
        public Transform Reference { get => reference; set => reference = value; }

        public bool CopyRotation { get => copyRotation; set => copyRotation = value; }
        public bool CopyPosition { get => copyPosition; set => copyPosition = value; }

        #endregion

        private void Awake()
        {
            if (reference != null)
            {
                startPosition = reference.localPosition;
                startRotation = reference.localRotation;
            }
        }

        private void FixedUpdate()
        {
            if (joint != null && reference != null)
            {
                UpdatePose();
            }
        }

        private void UpdatePose()
        {
            // Because you need to invert position and rotation before give it to joint
            // And you need this pose be relative to bone start pose (which is not 0, 0, 0 in skeleton) instead of bone local pose as well

            if (copyPosition)
            {
                joint.targetPosition = -(reference.localPosition - startPosition);
            }

            if (copyRotation)
            {
                // This is Inverse(reference.localRotation * Inverse(startRotation))
                joint.targetRotation = Quaternion.Inverse(reference.localRotation) * startRotation;
            }
        }
    }
}
