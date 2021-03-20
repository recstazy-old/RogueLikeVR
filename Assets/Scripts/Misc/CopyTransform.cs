using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class CopyTransform : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform target;

        [SerializeField]
        private bool position = true;

        [SerializeField]
        private bool rotation = true;

        #endregion

        #region Properties

        public Transform Target { get => target; set => target = value; }
        public bool Position { get => position; set => position = value; }
        public bool Rotation { get => rotation; set => rotation = value; }

        #endregion

        private void Update()
        {
            if (Target != null)
            {
                if (position)
                {
                    transform.position = target.position;
                }

                if (rotation)
                {
                    transform.rotation = target.rotation;
                }
            }
        }
    }
}
