using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.AniPhysics
{
    public class StabEffector : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        [Range(0f, 1f)]
        private float currentEffect = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        private float enabledEffect = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        private float disabledEffect = 0f;

        [SerializeField]
        private float enableTime = 1f;

        [SerializeField]
        private float disableTime = 1f;

        #endregion

        #region Properties

        public float Effect { get => currentEffect; set => currentEffect = value; }

        #endregion

        private void Awake()
        {
            Effect = enabledEffect;
        }

        public void SetEnabled(bool isEnabled)
        {
            if (gameObject.activeInHierarchy && enabled)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothChangeRoutine(isEnabled));
            }
        }

        private IEnumerator SmoothChangeRoutine(bool enable)
        {
            float endValue = enable ? enabledEffect : disabledEffect;
            float time = enable ? enableTime : disableTime;
            time = Mathf.Max(time, 0.001f);

            float startValue = currentEffect;
            float t = 0f;

            while (t <= 1f)
            {
                currentEffect = Mathf.Lerp(startValue, endValue, t);

                yield return null;
                t += Time.deltaTime / time;
            }

            currentEffect = endValue;
        }
    }
}
