using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    [System.Serializable]
    public struct JointDrivesSettings
    {
        #region Fields

        [SerializeField]
        private JointDrive linearX;

        [SerializeField]
        private JointDrive linearY;

        [SerializeField]
        private JointDrive linearZ;

        [SerializeField]
        private JointDrive angularX;

        [SerializeField]
        private JointDrive angularYZ;

        [SerializeField]
        private float connectedMassScale;

        #endregion

        #region Properties

        public JointDrive LinearX { get => linearX; set => linearX = value; }
        public JointDrive LinearY { get => linearY; set => linearY = value; }
        public JointDrive LinearZ { get => linearZ; set => linearZ = value; }
        public JointDrive AngularX { get => angularX; set => angularX = value; }
        public JointDrive AngularYZ { get => angularYZ; set => angularYZ = value; }
        public float ConnectedMassScale { get => connectedMassScale; set => connectedMassScale = value; }

        #endregion

        public JointDrivesSettings(ConfigurableJoint joint)
        {
            linearX = joint.xDrive;
            linearY = joint.yDrive;
            linearZ = joint.zDrive;
            angularX = joint.angularXDrive;
            angularYZ = joint.angularYZDrive;
            connectedMassScale = joint.connectedMassScale;
        }
    }
}
