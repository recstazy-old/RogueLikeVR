using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class Grip : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Collider[] colliders;

        #endregion

        #region Properties
		
        #endregion

        public void SetIgnoreCollisions(Collider other, bool shouldIgnore)
        {
            foreach (var c in colliders)
            {
                Physics.IgnoreCollision(other, c, shouldIgnore);
            }
        }
    }
}
