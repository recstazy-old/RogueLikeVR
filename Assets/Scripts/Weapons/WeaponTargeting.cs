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

        #endregion

        #region Properties

        public TargetPoint TargetPoint { get => targetPoint; set => targetPoint = value; }

        #endregion

        private void FixedUpdate()
        {
            if (weaponPose != null && TargetPoint != null)
            {
                weaponPose.LookAt(TargetPoint.transform);
            }
        }
    }
}
