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

        #endregion

        private void FixedUpdate()
        {
            if (weaponPose != null && targetPoint != null)
            {
                weaponPose.LookAt(targetPoint.transform);
            }
        }
    }
}
