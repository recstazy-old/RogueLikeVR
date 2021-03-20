using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameOn.UnityHelpers;

namespace RoguelikeVR
{
    public class DebalanceControl : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private UnityEvent onDebalanced;

        [SerializeField]
        private UnityEvent onStartingBalance;

        [SerializeField]
        private UnityEvent onBalanced;

        [SerializeField]
        private float startToBalanceTime;

        private BoneDebalanceDispatcher[] dispatchers;

        #endregion

        #region Properties

        public bool Debalanced { get; private set; }
        public bool StartedBalaning { get; private set; }

        #endregion

        private void Awake()
        {
            dispatchers = GetComponents<BoneDebalanceDispatcher>();

            foreach (var d in dispatchers)
            {
                d.OnDebalanced += BoneDebalanced;
            }
        }

        private void OnDestroy()
        {
            if (dispatchers != null)
            {
                foreach (var d in dispatchers)
                {
                    d.OnDebalanced -= BoneDebalanced;
                }
            }
        }

        public void StartBalancing()
        {
            if (Debalanced && !StartedBalaning)
            {
                onStartingBalance?.Invoke();
                StartedBalaning = true;
                this.RunDelayed(startToBalanceTime, SetBalanced);
            }
        }

        private void BoneDebalanced(BoneDebalanceDispatcher dispatcher)
        {
            if (!Debalanced)
            {
                Debalanced = true;
                onDebalanced?.Invoke();
            }
        }

        private void SetBalanced()
        {
            StartedBalaning = false;
            Debalanced = false;
            onBalanced?.Invoke();
        }
    }
}
