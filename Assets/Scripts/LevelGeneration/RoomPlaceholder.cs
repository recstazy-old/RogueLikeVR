using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public class RoomPlaceholder : MonoBehaviour
    {
        #region Fields

        private Room roomIstance;

        #endregion

        #region Properties
        
        public BoxCollider Bounds { get; private set; }
        public Exit[] Exits { get; private set; }
        public RoomNode Node { get; private set; }

        #endregion

        public void Setup(RoomNode node, Room prefab)
        {
            Node = node;
            roomIstance = Instantiate(prefab);
            roomIstance.transform.SetParent(transform);
            Bounds = roomIstance.Bounds;
            Exits = roomIstance.Exits;
        }
    }
}
