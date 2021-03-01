using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class DragBlend : MonoBehaviour, IBlendable
    {
        #region Fields

        [SerializeField]
        private ObjectFloatSlider blend;

        private Rigidbody body;
        private float defaultDrag;
        private float defaultAngularDrag;

        #endregion

        #region Properties

        public ObjectFloatSlider Blend { get => blend; set => blend = value; }
        public ObjectFloatSlider GlobalBlend { get; set; }

        #endregion

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            defaultDrag = body.drag;
            defaultAngularDrag = body.angularDrag;
        }

        private void FixedUpdate()
        {
            if (body != null && Blend != null)
            {
                var blend = GetTotalBlend();
                body.drag = Mathf.Lerp(0f, defaultDrag, blend);
                body.angularDrag = Mathf.Lerp(0.05f, defaultAngularDrag, blend);
            }
        }

        private float GetTotalBlend()
        {
            float total = Blend ? Blend.Value : 1f;

            if (GlobalBlend != null)
            {
                total *= GlobalBlend.Value;
            }

            return total;
        }
    }
}
