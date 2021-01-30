using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        public Dictionary<Exit, Tunnel> Tunnels { get; private set; }
        public GameObject View => view;

        #endregion

        public void RegenerateData()
        {
            RegenerateBounds();
            CacheExits();
        }

        public void SetTunnels(Dictionary<Exit, Tunnel> tunnels)
        {
            Tunnels = tunnels;
        }

        private void RegenerateBounds()
        {
            var bounds = gameObject.EncapsulateAllChildren();
            roomBounds.transform.position = bounds.center;
            roomBounds.size = bounds.size;
        }

        private void CacheExits()
        {
            exits = GetComponentsInChildren<Exit>();
        }
    }
}
