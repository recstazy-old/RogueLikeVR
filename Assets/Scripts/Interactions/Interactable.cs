using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class Interactable : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private UnityEvent onInteraction;

        #endregion

        #region Properties
    
        #endregion

        public void Interact()
        {
            onInteraction?.Invoke();
        }
    }
}
