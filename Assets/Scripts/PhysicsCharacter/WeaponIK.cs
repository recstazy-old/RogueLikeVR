using GameOn.UnityHelpers;
using RoguelikeVR.Interactions;
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

        [SerializeField]
        private bool useSecondaryGrip;

        private RigBuilder builder;
        private WeaponIKDoubler doubler;

        private UpdateOtherOnFixedUpdate mainHandUpdater;
        private TwoBoneIKConstraint mainHandIK;
        private Collider mainHandCollider;
        private FixedJoint mainHandJoint;

        private UpdateOtherOnFixedUpdate secondaryUpdater;
        private TwoBoneIKConstraint secondaryIK;
        private Collider secondaryCollider;
        private FixedJoint secondaryHandJoint;
        private int secondaryGripIndex = -1;

        #endregion

        #region Properties

        public WeaponIKDoubler WeaponDoubler => doubler;
        public bool Initialized => builder != null && mainHandIK != null;
        public Collider[] MainGripIgnore { get; set; }
        public Collider[] SecondGripIgnore { get; set; }

        #endregion

        public void Initialize(RigBuilder builder, TwoBoneIKConstraint mainHandIK, Collider mainHandCollider, TwoBoneIKConstraint secondaryIK, Collider secondaryCollider)
        {
            this.builder = builder;
            this.mainHandIK = mainHandIK;
            this.mainHandCollider = mainHandCollider;
            this.secondaryIK = secondaryIK;
            this.secondaryCollider = secondaryCollider;
        }

        public void SetWeapon(Weapon weapon)
        {
            if (weapon != null)
            {
                this.weapon = weapon;
                weapon.MainBody.isKinematic = true;
                secondaryGripIndex = weapon.SecondaryGripPoints.Length > 0 ? weapon.SecondaryGripPoints.RandomIndex() : -1;

                CreateWeaponIKDoubler();
                ConfigureMainGrip();
                ConfigureSecondaryGrip();

                this.WaitFramesAndRun(1, () => weapon.MainBody.isKinematic = false);
            }
            else
            {
                ReleaseMainGrip();
                ReleaseSecondaryGrip();
            }
        }

        public void ReleaseWeapon()
        {
            SetWeapon(null);
        }

        private void ConfigureMainGrip()
        {
            if (Initialized)
            {
                if (weapon.MainGrip != null)
                {
                    IgnoreCollidersByGrip(MainGripIgnore, weapon.MainGrip);

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
        }

        private void ConfigureSecondaryGrip()
        {
            if (Initialized && useSecondaryGrip)
            {
                if (weapon.SecondaryGripPoints.Length > 0)
                {
                    weapon.SecondaryGripPoints[secondaryGripIndex].SetIgnoreCollisions(secondaryCollider, true);
                    SetWeight(secondaryIK, 1f);
                    secondaryUpdater = doubler.SecondaryGrip.gameObject.AddComponent<UpdateOtherOnFixedUpdate>();
                    secondaryUpdater.Other = secondaryIK.data.target;

                    this.WaitUntilAndRun(
                        () => Vector3.Distance(secondaryCollider.transform.position, weapon.SecondaryGripPoints[secondaryGripIndex].transform.position) < 0.05f,
                        () =>
                        {
                            secondaryHandJoint = secondaryCollider.gameObject.AddComponent<FixedJoint>();
                            secondaryHandJoint.massScale = jointMassScale;
                            secondaryHandJoint.connectedBody = weapon.MainBody;
                        });

                    return;
                }
            }

            SetWeight(secondaryIK, 0f);
            secondaryIK?.gameObject?.SetActive(false);
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

        private void ReleaseSecondaryGrip()
        {
            if (Initialized)
            {
                SetWeight(secondaryIK, 0f);

                if (secondaryUpdater != null)
                {
                    Destroy(secondaryUpdater);
                }

                if (secondaryHandJoint != null)
                {
                    Destroy(secondaryHandJoint);
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

            if (secondaryGripIndex >= 0)
            {
                doubler.Initialize(weapon, mainGripReferencePoint, secondaryGripIndex);
            }
            else
            {
                doubler.Initialize(weapon, mainGripReferencePoint);
            }

            doubler.transform.SetParent(doublerParent);
        }

        private void IgnoreCollidersByGrip(Collider[] colliders, Grip grip)
        {
            if (colliders != null)
            {
                foreach (var c in colliders)
                {
                    grip.SetIgnoreCollisions(c, true);
                }
            }
        }
    }
}
