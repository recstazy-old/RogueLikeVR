using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RoguelikeVR.AI
{
    public class CharacterSetup : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Rigidbody movementBody;

        [SerializeField]
        private RagdollCharacter ragdoll;

        [SerializeField]
        private RigBuilder rigBuilder;

        [SerializeField]
        private TwoBoneIKConstraint mainHandIK;

        [SerializeField]
        private TwoBoneIKConstraint secondaryIK;

        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private TargetPoint startTargetPoint;

        [SerializeField]
        private Transform weaponPose;

        [SerializeField]
        private Transform weaponPoseParent;

        [SerializeField]
        private Collider[] mainGripIgnoreColliders;

        [SerializeField]
        private Collider[] secondGripIgnoreColliders;

        private IWeaponReciever weaponReciever;
        private ITargetPointReciever targetPointReciever;
        private WeaponIK weaponIK;
        private PhysicsMovementBase movement;

        #endregion

        #region Properties

        public float RagdollAnimationWeight { get => ragdoll ? ragdoll.GlobalBlendAmount : 1f; set => ragdoll?.SetGlobalBlend(value); }

        #endregion

        private void Awake()
        {
            weaponReciever = GetComponentInChildren<IWeaponReciever>();
            targetPointReciever = GetComponentInChildren<ITargetPointReciever>();
            weaponIK = GetComponentInChildren<WeaponIK>();
            movement = GetComponentInChildren<PhysicsMovementBase>();

            if (movement != null)
            {
                movement.Body = movementBody;
                movement.Initialize();
            }

            if (movementBody != null)
            {
                transform.SetParent(movementBody.transform);
            }

            weaponPose.transform.SetParent(weaponPoseParent);

            if (weaponIK != null)
            {
                weaponIK.MainGripIgnore = mainGripIgnoreColliders;
                weaponIK.SecondGripIgnore = secondGripIgnoreColliders;
                weaponIK.Initialize(rigBuilder, mainHandIK, ragdoll.RightHandCollider, secondaryIK, ragdoll.LeftHandCollider);
            }

            if (weaponReciever != null)
            {
                weaponReciever.SetWeapon(weapon);
            }

            if (targetPointReciever != null)
            {
                targetPointReciever.SetTargetPoint(startTargetPoint);
            }
        }
    }
}
