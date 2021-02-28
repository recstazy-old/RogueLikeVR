using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class DriveBlend : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private ObjectFloatSlider blend; 

        private ConfigurableJoint joint;
        private JointDrivesSettings defaultSettings;
        private JointDrive zeroDrive;

        #endregion

        #region Properties

        public float Blend => blend ? blend.Value : 1f;
        public ObjectFloatSlider GlobalBlend { get; set; }

        #endregion

        private void Awake()
        {
            joint = GetComponent<ConfigurableJoint>();
            
            if (joint != null)
            {
                defaultSettings = new JointDrivesSettings(joint);
            }
        }

        private void FixedUpdate()
        {
            UpdateWeight();
        }

        private void UpdateWeight()
        {
            float blend = GetTotalBlend();

            joint.xDrive = BlendDrives(zeroDrive, defaultSettings.LinearX, blend);
            joint.yDrive = BlendDrives(zeroDrive, defaultSettings.LinearY, blend);
            joint.zDrive = BlendDrives(zeroDrive, defaultSettings.LinearZ, blend);

            joint.angularXDrive = BlendDrives(zeroDrive, defaultSettings.AngularX, blend);
            joint.angularYZDrive = BlendDrives(zeroDrive, defaultSettings.AngularYZ, blend);

            joint.massScale = Mathf.Lerp(1f, defaultSettings.MassScale, blend);
            joint.connectedMassScale = Mathf.Lerp(1f, defaultSettings.ConnectedMassScale, blend);
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
            float total = Blend;

            if (GlobalBlend != null)
            {
                total *= GlobalBlend.Value;
            }

            return total;
        }
    }
}
