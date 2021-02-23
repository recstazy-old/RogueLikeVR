using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.PhysicsCharacters
{
    public class MoveReferenceToTarget : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Rigidbody referenceBody;

        [SerializeField]
        private Rigidbody targetBody;

        #endregion

        #region Properties
		
        #endregion

        public void Execute()
        {
            var position = targetBody.transform.position;
            position.y = referenceBody.transform.position.y;

            referenceBody.MovePosition(position);
        }
    }
}
