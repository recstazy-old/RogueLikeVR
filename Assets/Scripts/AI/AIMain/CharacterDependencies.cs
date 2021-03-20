using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using RoguelikeVR.Weapons;

namespace RoguelikeVR.AI
{
    [System.Serializable]
    public class CharacterDependencies
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
        private TwoBoneIKConstraint headIK;

        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private TargetPoint startTargetPoint;

        [SerializeField]
        private Transform weaponPose;

        [SerializeField]
        private Transform weaponPosePosition;

        [SerializeField]
        private Collider[] mainGripIgnoreColliders;

        [SerializeField]
        private Collider[] secondGripIgnoreColliders;

        #endregion

        #region Properties

        public Rigidbody MovementBody { get => movementBody; set => movementBody = value; }
        public RagdollCharacter Ragdoll { get => ragdoll; set => ragdoll = value; }
        public RigBuilder RigBuilder { get => rigBuilder; set => rigBuilder = value; }
        public TwoBoneIKConstraint MainHandIK { get => mainHandIK; set => mainHandIK = value; }
        public TwoBoneIKConstraint SecondaryIK { get => secondaryIK; set => secondaryIK = value; }
        public TwoBoneIKConstraint HeadIK { get => headIK; set => headIK = value; }
        public Weapon Weapon { get => weapon; set => weapon = value; }
        public TargetPoint StartTargetPoint { get => startTargetPoint; set => startTargetPoint = value; }
        public Transform WeaponPose { get => weaponPose; set => weaponPose = value; }
        public Transform WeaponPosePosition { get => weaponPosePosition; set => weaponPosePosition = value; }
        public Collider[] MainGripIgnoreColliders { get => mainGripIgnoreColliders; set => mainGripIgnoreColliders = value; }
        public Collider[] SecondGripIgnoreColliders { get => secondGripIgnoreColliders; set => secondGripIgnoreColliders = value; }

        #endregion
    }
}
