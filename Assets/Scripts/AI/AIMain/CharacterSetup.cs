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
        private Weapon weapon;

        [SerializeField]
        private TargetPoint startTargetPoint;

        private IWeaponReciever weaponReciever;
        private ITargetPointReciever targetPointReciever;
        private WeaponIK weaponIK;
        private PhysicsMovementBase movement;

        #endregion

        #region Properties

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

            if (weaponIK != null)
            {
                weaponIK.Initialize(rigBuilder, mainHandIK, ragdoll.RightHandCollider);
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
