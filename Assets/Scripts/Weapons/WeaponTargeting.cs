using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class WeaponTargeting : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform weaponPose;

        [SerializeField]
        private Transform targetPoint;

        #endregion

        #region Properties

        #endregion

        private void FixedUpdate()
        {
            if (weaponPose != null && targetPoint != null)
            {
                weaponPose.LookAt(targetPoint);
            }
        }
    }
}
