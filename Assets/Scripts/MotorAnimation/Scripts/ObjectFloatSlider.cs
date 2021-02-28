using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class ObjectFloatSlider : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        [Range(0f, 1f)]
        protected float value = 1f;

        #endregion

        #region Properties

        public float Value { get => Mathf.Clamp01(value); set => this.value = Mathf.Clamp01(value); }

        #endregion
    }
}
