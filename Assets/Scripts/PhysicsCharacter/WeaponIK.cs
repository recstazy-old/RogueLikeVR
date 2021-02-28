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
        [EditorReadOnly]
        private Weapon weapon;

        [SerializeField]
        private WeaponIKDoubler doublerPrefab;

        [SerializeField]
        private Transform doublerParent;
        
        [SerializeField]
        private Transform mainGripReferencePoint;

        [SerializeField]
        private float jointMassScale;

        private UpdateOtherOnFixedUpdate mainHandUpdater;
        private FixedJoint mainHandJoint;
        private WeaponIKDoubler doubler;

        private RigBuilder builder;
        private TwoBoneIKConstraint mainHandIK;
        private Collider mainHandCollider;

        #endregion

        #region Properties

        public WeaponIKDoubler WeaponDoubler => doubler;
        public bool Initialized => builder != null && mainHandIK != null;

        #endregion

        public void Initialize(RigBuilder builder, TwoBoneIKConstraint mainHandIK, Collider mainHandCollider)
        {
            this.builder = builder;
            this.mainHandIK = mainHandIK;
            this.mainHandCollider = mainHandCollider;
        }

        public void SetWeapon(Weapon weapon)
        {
            if (weapon != null)
            {
                this.weapon = weapon;
                weapon.MainBody.isKinematic = true;
                CreateWeaponIKDoubler();

                if (weapon.MainGrip != null)
                {
                    ConfigureMainGrip(weapon);
                }

                Debug.Break();

                this.WaitFramesAndRun(1, () => weapon.MainBody.isKinematic = false);
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
            if (Initialized)
            {
                weapon.MainGrip.SetIgnoreCollisions(mainHandCollider, true);
                SetWeight(mainHandIK, 1f);
                mainHandUpdater = doubler.MainGripPoint.gameObject.AddComponent<UpdateOtherOnFixedUpdate>();
                mainHandUpdater.Other = mainHandIK.data.target;

                weapon.transform.rotation = Quaternion.identity;
                weapon.transform.rotation *= mainHandCollider.transform.rotation * Quaternion.Inverse(weapon.MainGrip.transform.rotation);

                var deltaPosition = mainHandCollider.transform.position - weapon.MainGrip.transform.position;
                weapon.transform.position += deltaPosition;

                mainHandJoint = mainHandCollider.gameObject.AddComponent<FixedJoint>();
                mainHandJoint.massScale = jointMassScale;
                mainHandJoint.connectedBody = weapon.MainBody;
            }
        }

        private void ReleaseMainGrip()
        {
            if (Initialized)
            {
                SetWeight(mainHandIK, 0f);

                if (mainHandUpdater != null)
                {
                    Destroy(mainHandUpdater);
                }

                if (mainHandJoint != null)
                {
                    Destroy(mainHandJoint);
                }
            }
        }

        private void SetWeight(TwoBoneIKConstraint ik, float weight)
        {
            builder.enabled = false;
            ik.weight = weight;
            builder.enabled = true;
        }

        private void CreateWeaponIKDoubler()
        {
            if (doubler != null)
            {
                Destroy(doubler.gameObject);
            }

            doubler = Instantiate(doublerPrefab);
            doubler.Initialize(weapon, mainGripReferencePoint);
            doubler.transform.SetParent(doublerParent);
        }
    }
}
