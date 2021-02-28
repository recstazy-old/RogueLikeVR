using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class BoneDebalanceDispatcher : MonoBehaviour
    {
        public event System.Action<BoneDebalanceDispatcher> OnDebalanced;

        #region Fields

        [SerializeField]
        private Transform reference;

        [SerializeField]
        private Transform target;

        [SerializeField]
        private float debalanceAngle = 30f;

        #endregion

        #region Properties

        #endregion

        private void Update()
        {
            CheckDebalance();
        }

        private void CheckDebalance()
        {
            var angle = Vector3.Angle(reference.up, target.up);

            if (angle >= debalanceAngle)
            {
                OnDebalanced?.Invoke(this);
            }
        }
    }
}
