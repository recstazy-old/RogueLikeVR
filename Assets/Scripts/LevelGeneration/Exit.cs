using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class Exit : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Vector2 size;

        #endregion

        #region Properties

        public Door Door { get; private set; }
        public Vector2 Size => size;

        #endregion
    }
}
