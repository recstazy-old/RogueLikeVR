using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class KeyToUnityEvent : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private KeyCode key;

        [SerializeField]
        private UnityEvent onDown;

        #endregion

        #region Properties

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                onDown?.Invoke();
            }
        }
    }
}
