using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class DriveBlend : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        [Range(0f, 1f)]
        private float blend = 1f;

        #endregion

        #region Properties

        public float Blend { get => blend; set => blend = value; }

        #endregion
    }
}
