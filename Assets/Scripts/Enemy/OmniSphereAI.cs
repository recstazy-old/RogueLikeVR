using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class OmniSphereAI : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AIBase aiBase;

        [SerializeField]
        private float updateTargetDelay;

        private Transform target;

        #endregion

        #region Properties
    
        #endregion

        public void AgressionTriggered(Collider other, bool isEnter)
        {
            if (isEnter)
            {
                if (target == null)
                {
                    target = other.transform;
                    StartFollowingTarget();
                }
            }
            else
            {
                if (target == other.transform)
                {
                    target = null;
                }
            }
        }

        private void StartFollowingTarget()
        {
            StopAllCoroutines();
            StartCoroutine(FollowRoutine());
        }

        private IEnumerator FollowRoutine()
        {
            var delay = new WaitForSeconds(updateTargetDelay);

            while (target != null)
            {
                yield return delay;

                if (target != null)
                {
                    aiBase.MoveTo(target.position);
                }
            }
        }
    }
}
