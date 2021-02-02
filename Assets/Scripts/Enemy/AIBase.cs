using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RoguelikeVR
{
    public class AIBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private NavMeshAgent navAgent;

        #endregion

        #region Properties
    
        #endregion

        public void MoveTo(Vector3 position)
        {
            navAgent.SetDestination(position);
        }
    }
}
