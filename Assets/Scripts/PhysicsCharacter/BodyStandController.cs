using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class BodyStandController : MonoBehaviour
    {
        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        #region Fields

        [SerializeField]
        private Rigidbody rootBody;

        [SerializeField]
        private Rigidbody balancingBody;

        [SerializeField]
        private float debalanceDistance;

        [SerializeField]
        private float debalanceAngle;

        [SerializeField]
        private float sleepVelocityTreshold;

        [SerializeField]
        private float sleepingTime = 3f;

        [SerializeField]
        private float standingUpTime = 2f;

        [SerializeField]
        private LayerMask groundLayers = -1;

        [SerializeField]
        private BoolEvent onStandingChanged;

        [SerializeField]
        private UnityEvent onBeforeStandUp;

        [SerializeField]
        private UnityEvent onStandUp;

        [SerializeField]
        private UnityEvent onFall;

        private List<Collider> groundColliders = new List<Collider>();
        private Coroutine sleepRoutine;

        #endregion

        #region Properties

        public bool IsGrounded => groundColliders.Count > 0;
        public bool IsBalancing { get; private set; } = true;
        public bool IsStanding { get; private set; } = true;
        public bool IsSleeping => sleepRoutine != null;
        public bool IsStandingUp { get; private set; }

        #endregion

        private void Update()
        {
            CheckDebalance();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (groundLayers.ContainsLayer(collision.collider.gameObject.layer))
            {
                if (!groundColliders.Contains(collision.collider))
                {
                    groundColliders.Add(collision.collider);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (groundLayers.ContainsLayer(collision.collider.gameObject.layer))
            {
                if (groundColliders.Contains(collision.collider))
                {
                    groundColliders.Remove(collision.collider);
                }
            }
        }

        private void CheckDebalance()
        {
            if (!IsStandingUp)
            {
                bool lastBalancing = IsBalancing;
                Vector3 bodiesDelta = balancingBody.position - rootBody.position;
                var bodiesDeltaXZ = bodiesDelta;
                bodiesDeltaXZ.y = 0f;
                bool debalancedByDistance = bodiesDeltaXZ.magnitude >= debalanceDistance;

                var balanceToUpAngle = Vector3.Angle(Vector3.up, bodiesDelta.normalized);
                bool debalancedByAngle = balanceToUpAngle >= debalanceAngle;

                var isBalancing = !(debalancedByDistance || debalancedByAngle);

                if (isBalancing != lastBalancing)
                {
                    BalanceChanged(isBalancing);
                }
            }
        }

        private void BalanceChanged(bool isBalancing)
        {
            bool newIsStanding = isBalancing && IsGrounded;

            if (newIsStanding != IsStanding)
            {
                if (!newIsStanding)
                {
                    IsStanding = false;

                    if (!IsSleeping)
                    {
                        sleepRoutine = StartCoroutine(SleepRoutine());
                        onStandingChanged?.Invoke(IsStanding);
                        onFall?.Invoke();
                    }
                }
            }
        }

        private IEnumerator SleepRoutine()
        {
            float timeSlept = 0f;

            while (timeSlept <= sleepingTime)
            {
                if (rootBody.velocity.magnitude < sleepVelocityTreshold && rootBody.angularVelocity.magnitude < sleepVelocityTreshold)
                {
                    timeSlept += Time.deltaTime;
                }

                yield return null;
            }

            IsStandingUp = true;
            onBeforeStandUp?.Invoke();

            yield return new WaitForSeconds(standingUpTime);
            IsStandingUp = false;
            sleepRoutine = null;

            IsStanding = true;
            onStandingChanged?.Invoke(true);
            onStandUp?.Invoke();
        }
    }
}
