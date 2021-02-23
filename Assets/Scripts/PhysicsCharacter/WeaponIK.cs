using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RoguelikeVR.Weapons
{
    public class WeaponIK : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private TwoBoneIKConstraint rightIK;

        [SerializeField]
        private Collider rightHandCollider;

        [SerializeField]
        private bool createJoints;

        private UpdateOtherOnFixedUpdate rightUpdater;
        private FixedJoint rightJoint;

        #endregion

        #region Properties

        #endregion

        public void SetWeapon(Weapon weapon)
        {
            if (weapon != null)
            {
                this.weapon = weapon;
                weapon.MainBody.isKinematic = true;

                if (weapon.MainGrip != null)
                {
                    ConfigureMainGrip(weapon);
                }

                this.WaitFramesAndRun(2, () => weapon.MainBody.isKinematic = false);
            }
            else
            {
                ReleaseMainGrip();
            }
        }

        public void ReleaseWeapon()
        {
            SetWeapon(null);
        }

        private void ConfigureMainGrip(Weapon weapon)
        {
            weapon.MainGrip.SetIgnoreCollisions(rightHandCollider, true);
            rightIK.weight = 1f;
            rightUpdater = weapon.MainGrip.gameObject.AddComponent<UpdateOtherOnFixedUpdate>();
            rightUpdater.Other = rightIK.data.target;

            if (createJoints)
            {
                this.WaitFramesAndRun(3, () =>
                {
                    rightJoint = rightHandCollider.gameObject.AddComponent<FixedJoint>();
                    rightJoint.connectedBody = weapon.MainBody;
                });
            }
        }

        private void ReleaseMainGrip()
        {
            rightIK.weight = 0f;

            if (rightUpdater != null)
            {
                Destroy(rightUpdater);
            }

            if (rightJoint != null)
            {
                Destroy(rightJoint);
            }
        }
    }
}
