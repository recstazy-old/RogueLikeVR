using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.AniPhysics
{
    public class StabEffector : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        [EditorReadOnly]
        [Range(0f, 1f)]
        private float currentEffect = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        private float enabledEffect = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        private float disabledEffect = 0f;

        [SerializeField]
        private float smoothTime = 1f;

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
            StopAllCoroutines();
            StartCoroutine(SmoothChangeRoutine(isEnabled ? enabledEffect : disabledEffect));
        }

        private IEnumerator SmoothChangeRoutine(float endValue)
        {
            float startValue = currentEffect;
            float t = 0f;

            while (t <= 1f)
            {
                currentEffect = Mathf.Lerp(startValue, endValue, t);

                yield return null;
                t += Time.deltaTime / smoothTime;
            }
        }
    }
}
