using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR.Interactions
{
    public class InteractionHandler : MonoBehaviour
    {
        #region Fields

        private HashSet<Collider> interactions = new HashSet<Collider>();

        #endregion

        #region Properties

        public bool CanInteract => Current != null;
        public Interactable Current { get; private set; }

        #endregion

        public void TryInteract()
        {
            Current?.Interact();
        }

        public void InteractionTriggered(Collider trigger, bool isEnter)
        {
            if (isEnter)
            {
                interactions.Add(trigger);
            }
            else
            {
                interactions.Remove(trigger);
            }

            UpdateCurrent();
        }

        private void UpdateCurrent()
        {
            if (interactions.Count > 0)
            {
                Current = interactions.OrderBy(i => Vector3.Distance(i.transform.position, transform.position)).First().GetComponent<Interactable>();
            }
            else
            {
                Current = null;
            }
        }
    }
}
