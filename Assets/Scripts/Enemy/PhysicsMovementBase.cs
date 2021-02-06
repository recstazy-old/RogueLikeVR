using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RoguelikeVR
{
    public class PhysicsMovementBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private NavMeshAgent navAgent;

        [SerializeField]
        private float navAgentWaitDistance;

        private IPhysicsMovement movementImplementation;

        #endregion

        #region Properties

        public Rigidbody Body => body;
        public NavMeshAgent NavAgent => navAgent;
        public float NavAgentWaitDistance { get => navAgentWaitDistance; set => navAgentWaitDistance = value; }

        #endregion

        private void Awake()
        {
            movementImplementation = GetComponent<IPhysicsMovement>();

            if (navAgent != null)
            {
                navAgent.gameObject.name = body.gameObject.name + "_NavAgent";
                navAgent.transform.SetParent(body.transform.parent);
            }
        }

        private void FixedUpdate()
        {
            if (body != null && navAgent != null)
            {
                navAgent.isStopped = Vector3.Distance(body.position, navAgent.transform.position) > navAgentWaitDistance;
                movementImplementation?.MoveBody(body, navAgent.transform);
            }
        }

        public void MoveTo(Vector3 target)
        {
            if (navAgent != null)
            {
                navAgent.ResetPath();
                navAgent.SetDestination(target);
            }
        }
    }
}
