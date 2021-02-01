using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace RoguelikeVR
{
    public class Room : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private BoxCollider roomBounds;

        [SerializeField]
        private Exit[] exits;

        [SerializeField]
        private GameObject view;

        #endregion

        #region Properties

        public BoxCollider Bounds => roomBounds;
        public Exit[] Exits => exits;
        public GameObject View => view;

        #endregion

        public void RegenerateData()
        {
            RegenerateBounds();
            CacheExits();
        }

        private void RegenerateBounds()
        {
            if (roomBounds == null)
            {
                var boundsBox = GetComponentsInChildren<BoxCollider>().FirstOrDefault(r => r.gameObject.layer == LayerMask.NameToLayer("Bounds"));
                roomBounds = boundsBox;
            }

            if (roomBounds != null)
            {
                var bounds = gameObject.EncapsulateAllChildren();
                roomBounds.transform.position = bounds.center;
                roomBounds.size = bounds.size;
            }
        }

        private void CacheExits()
        {
            exits = GetComponentsInChildren<Exit>();
        }
    }
}
