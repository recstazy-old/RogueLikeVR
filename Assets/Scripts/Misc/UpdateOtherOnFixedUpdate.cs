using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RoguelikeVR
{
    public class UpdateOtherOnFixedUpdate : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform other;

        [SerializeField]
        private bool position = true;

        [SerializeField]
        private bool rotation = true;

        #endregion

        #region Properties

        public Transform Other { get => other; set => other = value; }
        public bool Position { get => position; set => position = value; }
        public bool Rotation { get => rotation; set => rotation = value; }

        #endregion

        private void FixedUpdate()
        {
            if (Other != null)
            {
                if (Position)
                {
                    Other.position = transform.position;
                }

                if (Rotation)
                {
                    Other.rotation = transform.rotation;
                }
            }
        }
    }
}
