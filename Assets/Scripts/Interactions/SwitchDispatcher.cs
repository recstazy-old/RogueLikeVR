using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR.Interactions
{
    public class SwitchDispatcher : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private UnityEvent onSwitchedOn;

        [SerializeField]
        private UnityEvent onSwitchedOff;

        [SerializeField]
        private bool startState;

        [SerializeField]
        [EditorReadOnly]
        private bool currentState;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            currentState = startState;
        }

        public void Switch()
        {
            currentState = !currentState;

            if (currentState == true)
            {
                onSwitchedOn?.Invoke();
            }
            else
            {
                onSwitchedOff?.Invoke();
            }
        }
    }
}
