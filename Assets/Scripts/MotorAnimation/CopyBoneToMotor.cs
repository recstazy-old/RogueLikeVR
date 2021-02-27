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
        private bool inverseRotation = true;

        [SerializeField]
        private bool copyRotation = true;

        [SerializeField]
        private bool copyPosition = true;

        [SerializeField]
        private bool inversePosition = true;

        private Vector3 startPosition;
        private Quaternion startRotation;

        #endregion

        #region Properties

        public ConfigurableJoint Joint { get => joint; set => joint = value; }
        public Transform Reference { get => reference; set => reference = value; }

        #endregion

        private void Awake()
        {
            startPosition = reference.localPosition;
            startRotation = reference.localRotation;
        }

        private void FixedUpdate()
        {
            if (joint != null && reference != null)
            {
                if (copyPosition)
                {
                    var newPosition = inversePosition ? -reference.localPosition : reference.localPosition;
                    joint.targetPosition = newPosition - (inversePosition ? -startPosition : startPosition);
                }

                if (copyRotation)
                {
                    Quaternion newRotation = inverseRotation ? Quaternion.Inverse(reference.localRotation) : reference.localRotation;
                    joint.targetRotation = newRotation * Quaternion.Inverse(inverseRotation ? Quaternion.Inverse(startRotation) : startRotation);
                }
            }
        }
    }
}
