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
        private DriveBlend driveBlend;

        [SerializeField]
        private bool copyRotation = true;

        [SerializeField]
        private bool copyPosition = false;

        private JointDrivesSettings defaultSettings;
        private JointDrive zeroDrive;
        private Vector3 startPosition;
        private Quaternion startRotation;

        #endregion

        #region Properties

        public ConfigurableJoint Joint { get => joint; set => joint = value; }
        public Transform Reference { get => reference; set => reference = value; }
        public DriveBlend Blend { get => driveBlend; set => driveBlend = value; }
        public DriveBlend GlobalBlend { get; set; }

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
            
            if (joint != null)
            {
                defaultSettings = new JointDrivesSettings(joint);
            }

            CheckBlendValid();
        }

        private void FixedUpdate()
        {
            if (joint != null && reference != null)
            {
                UpdatePose();
                UpdateWeight();
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

        private void UpdateWeight()
        {
            float blend = copyPosition ? GetTotalBlend() : 0f;
            joint.xDrive = BlendDrives(zeroDrive, defaultSettings.LinearX, blend);
            joint.yDrive = BlendDrives(zeroDrive, defaultSettings.LinearY, blend);
            joint.zDrive = BlendDrives(zeroDrive, defaultSettings.LinearZ, blend);

            blend = copyRotation ? GetTotalBlend() : 0f;
            joint.angularXDrive = BlendDrives(zeroDrive, defaultSettings.AngularX, blend);
            joint.angularYZDrive = BlendDrives(zeroDrive, defaultSettings.AngularYZ, blend);
        }

        private void CheckBlendValid()
        {
            if (Blend == null)
            {
                var blend = GetComponent<DriveBlend>();

                if (blend == null)
                {
                    blend = gameObject.AddComponent<DriveBlend>();
                }

                Blend = blend;
            }
        }

        private JointDrive BlendDrives(JointDrive left, JointDrive right, float alpha)
        {
            var drive = new JointDrive();
            drive.positionSpring = Mathf.Lerp(left.positionSpring, right.positionSpring, alpha);
            drive.positionDamper = Mathf.Lerp(left.positionDamper, right.positionDamper, alpha);
            drive.maximumForce = Mathf.Lerp(left.maximumForce, right.maximumForce, alpha);
            return drive;
        }

        private float GetTotalBlend()
        {
            float blend = 1f;

            if (Blend != null)
            {
                blend *= Blend.Blend;
            }

            if (GlobalBlend != null)
            {
                blend *= GlobalBlend.Blend;
            }

            return blend;
        }
    }
}
