using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class CallDelayed : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private UnityEvent whatToCall;

        [SerializeField]
        private float delay;

        #endregion

        #region Properties
		
        #endregion

        public void Call()
        {
            this.RunDelayed(delay, () => whatToCall?.Invoke());
        }
    }
}
