using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class IgnoreCollisionWithGameObject : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject target;

        [SerializeField]
        private bool ignore = true;

        #endregion

        #region Properties

        public GameObject Target { get => target; set => target = value; }

        #endregion

        private void Awake()
        {
            var thisCollider = GetComponent<Collider>();

            if (thisCollider != null)
            {
                var others = target.GetComponentsInChildren<Collider>();

                foreach (var o in others)
                {
                    Physics.IgnoreCollision(o, thisCollider, ignore);
                }
            }
        }
    }
}
