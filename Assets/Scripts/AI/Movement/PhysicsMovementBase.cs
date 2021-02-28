using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RoguelikeVR.AI
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

        [SerializeField]
        private bool canMoveonStart;

        private IPhysicsMovement movementImplementation;
        private bool initialized;

        #endregion

        #region Properties

        public NavMeshAgent NavAgent => navAgent;
        public float NavAgentWaitDistance { get => navAgentWaitDistance; set => navAgentWaitDistance = value; }
        public bool CanMove { get; private set; }
        public Rigidbody Body { get => body; set => body = value; }

        #endregion

        public void Initialize()
        {
            if (!initialized)
            {
                initialized = true;
                movementImplementation = GetComponent<IPhysicsMovement>();

                if (navAgent != null)
                {
                    navAgent.gameObject.name = Body.gameObject.name + "_NavAgent";
                    navAgent.transform.SetParent(Body.transform.parent);
                }

                SetCanMove(canMoveonStart);
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                if (Body != null && navAgent != null)
                {
                    navAgent.isStopped = Vector3.Distance(Body.position, navAgent.transform.position) > navAgentWaitDistance;
                    movementImplementation?.MoveBody(Body, navAgent.transform);
                }
            }
        }

        public void SetCanMove(bool canMove)
        {
            if (canMove != CanMove)
            {
                CanMove = canMove;
                
                if (!CanMove)
                {
                    StopMovement();
                }

                if (movementImplementation != null)
                {
                    movementImplementation.IsActive = CanMove;
                }
            }
        }

        public void StopMovement()
        {
            navAgent.ResetPath();
            navAgent.isStopped = true;
            navAgent.transform.position = Body.position;
            navAgent.isStopped = false;
        }

        public void MoveTo(Vector3 target)
        {
            if (navAgent != null)
            {
                navAgent.ResetPath();
                navAgent.SetDestination(target);
            }
        }

        public void LookAt(Transform target)
        {
            if (movementImplementation is IPhysicsLookAt)
            {
                (movementImplementation as IPhysicsLookAt)?.MoveBodyRotation(Body, target);
            }
        }
    }
}
