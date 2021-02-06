using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameOn.UnityHelpers;

namespace RoguelikeVR
{
    public class AIBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private PhysicsMovementBase movement;

        #endregion

        #region Properties
    
        #endregion

        public void MoveTo(Vector3 position)
        {
            movement.MoveTo(position);
        }
    }
}
