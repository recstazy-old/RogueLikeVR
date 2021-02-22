using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class BodyFallController : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Rigidbody controlledBody;

        [SerializeField]
        private Joint[] jointsMassScaleDependent;

        [SerializeField]
        private float massScale = 100f;

        [SerializeField]
        private float standUpTime;

        private RigidbodyConstraints defaultConstraints;
        private bool isStandingNow;
        private Coroutine standUpRoutine;

        #endregion

        #region Properties

        public bool IsStandingUp => standUpRoutine != null;

        #endregion

        private void Awake()
        {
            if (controlledBody != null)
            {
                defaultConstraints = controlledBody.constraints;
            }
        }

        public void StandingChanged(bool isStanding)
        {
            if (controlledBody != null)
            {
                isStandingNow = isStanding;

                if (isStandingNow && !IsStandingUp)
                {
                    StandUp();
                }
                else
                {
                    Fall();
                }
            }
        }

        private void StandUp()
        {
            StopAllCoroutines();

            foreach (var j in jointsMassScaleDependent)
            {
                j.massScale = 1f / massScale;
                j.connectedMassScale = massScale;
            }

            standUpRoutine = StartCoroutine(StandUpRoutine());
        }

        private void Fall()
        {
            StopAllCoroutines();

            foreach (var j in jointsMassScaleDependent)
            {
                j.massScale = massScale;
                j.connectedMassScale = 1f / massScale;
            }

            controlledBody.constraints = RigidbodyConstraints.None;
        }

        private IEnumerator StandUpRoutine()
        {
            var waitFixedUpdate = new WaitForFixedUpdate();
            float t = 0f;
            var newForward = controlledBody.transform.forward;
            newForward.y = 0f;
            var targetRotation = Quaternion.LookRotation(newForward.normalized, Vector3.up);

            while (controlledBody != null && isStandingNow && t <= 1f)
            {
                var newRotation = Quaternion.Lerp(controlledBody.rotation, targetRotation, t);
                controlledBody.MoveRotation(newRotation);

                yield return waitFixedUpdate;
                t += Time.fixedDeltaTime / standUpTime;
            }

            controlledBody.MoveRotation(targetRotation);
            controlledBody.constraints = defaultConstraints;

            foreach (var j in jointsMassScaleDependent)
            {
                j.massScale = 1f;
                j.connectedMassScale = 1f;
            }

            standUpRoutine = null;
        }
    }
}
