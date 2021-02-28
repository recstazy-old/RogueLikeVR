using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class DebalanceControl : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private UnityEvent onDebalanced;

        private BoneDebalanceDispatcher[] dispatchers;

        #endregion

        #region Properties

        public bool Debalanced { get; private set; }

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

        public void ResetDebalance()
        {
            Debalanced = false;
        }

        private void BoneDebalanced(BoneDebalanceDispatcher dispatcher)
        {
            if (!Debalanced)
            {
                Debalanced = true;
                onDebalanced?.Invoke();
            }
        }
    }
}
