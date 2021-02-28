using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class RagdollCharacter : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        [Range(0f, 1f)]
        private float startBlend = 1f;

        [SerializeField]
        private Collider leftHandCollider;

        [SerializeField]
        private Collider rightHandCollider;

        [SerializeField]
        private Collider leftFootCollider;

        [SerializeField]
        private Collider rightFootCollider;

        [SerializeField]
        private Collider headCollider;

        #endregion

        #region Properties

        public Collider LeftHandCollider { get => leftHandCollider; }
        public Collider RightHandCollider { get => rightHandCollider; }
        public Collider LeftFootCollider { get => leftFootCollider; }
        public Collider RightFootCollider { get => rightFootCollider; }
        public Collider HeadCollider { get => headCollider; }
        public ObjectFloatSlider GlobalBlend { get; private set; }

        #endregion

        private void Awake()
        {
            GlobalBlend = gameObject.AddComponent<ObjectFloatSlider>();
            GlobalBlend.Value = startBlend;
            var blends = GetComponentsInChildren<DriveBlend>();

            foreach (var b in blends)
            {
                b.GlobalBlend = GlobalBlend;
            }
        }
    }
}
