using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.Weapons
{
    public class WeaponTargeting : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform weaponPose;

        [SerializeField]
        private TargetPoint targetPoint;

        [SerializeField]
        private float lookingConeAngle;

        #endregion

        #region Properties

        public TargetPoint TargetPoint { get => targetPoint; set => targetPoint = value; }

        #endregion

        private void FixedUpdate()
        {
            if (weaponPose != null && TargetPoint != null)
            {
                var delta = TargetPoint.transform.position - weaponPose.position;
                var angle = Vector3.Angle(delta.normalized, transform.forward);

                if (angle < lookingConeAngle)
                {
                    weaponPose.LookAt(TargetPoint.transform);
                }
                else
                {
                    weaponPose.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }
}
