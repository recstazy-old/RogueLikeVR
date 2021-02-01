using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public class RoomPlaceholder : MonoBehaviour
    {
        #region Fields

        private Room roomInstance;

        #endregion

        #region Properties
        
        public BoxCollider Bounds { get; private set; }
        public Exit[] Exits { get; private set; }
        public RoomNode Node { get; private set; }
        public Room RoomInstance => roomInstance;

        #endregion

        public void Setup(RoomNode node, Room prefab)
        {
            Node = node;
            roomInstance = Instantiate(prefab);
            roomInstance.transform.SetParent(transform);
            Bounds = roomInstance.Bounds;
            Exits = roomInstance.Exits;
        }
    }
}
