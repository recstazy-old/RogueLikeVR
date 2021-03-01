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
        private float blendTime = 1f;

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
        public float GlobalBlendAmount => GlobalBlend ? GlobalBlend.Value : 1f;

        #endregion

        private void Awake()
        {
            GlobalBlend = gameObject.AddComponent<ObjectFloatSlider>();
            GlobalBlend.Value = startBlend;
            var blends = GetComponentsInChildren<IBlendable>();

            foreach (var b in blends)
            {
                b.GlobalBlend = GlobalBlend;
            }
        }

        public void SetGlobalBlend(float blend)
        {
            if (GlobalBlend != null)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothChangeBlendRoutine(blend));
            }
        }

        private IEnumerator SmoothChangeBlendRoutine(float targetValue)
        {
            float currentValue = GlobalBlend.Value;
            float t = 0f;

            while (t <= 1f)
            {
                GlobalBlend.Value = Mathf.Lerp(currentValue, targetValue, t);

                yield return null;
                t += Time.deltaTime / blendTime;
            }

            GlobalBlend.Value = targetValue;
        }
    }
}
