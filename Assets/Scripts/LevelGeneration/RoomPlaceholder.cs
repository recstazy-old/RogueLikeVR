using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public class RoomPlaceholder : MonoBehaviour
    {
        #region Fields
    
        #endregion

        #region Properties
        
        public BoxCollider Bounds { get; private set; }
        public Transform[] Exits { get; private set; }
        public RoomNode Node { get; private set; }

        #endregion

        public void Setup(RoomNode node, Room prefab, Exit[] exits)
        {
            Node = node;

            var boundsObject = new GameObject("Bounds");
            boundsObject.transform.SetParent(transform);
            Bounds = boundsObject.AddComponent<BoxCollider>();
            Bounds.isTrigger = true;
            Bounds.gameObject.layer = LayerMask.NameToLayer("Bounds");
            Bounds.transform.localPosition = prefab.Bounds.transform.localPosition;
            Bounds.size = prefab.Bounds.size;

            var exitsParent = new GameObject("Exits");
            exitsParent.transform.SetParent(transform);
            Exits = new Transform[exits.Length];

            for(int i = 0; i < exits.Length; i++)
            {
                var exit = exits[i];
                var exitObject = new GameObject(exit.name + "_Placeholder");
                exitObject.transform.SetParent(exitsParent.transform);
                exitObject.transform.localPosition = exit.transform.localPosition;
                exitObject.transform.localRotation = exit.transform.localRotation;
                exitObject.transform.localScale = (Vector3)exit.Size + Vector3.forward;
                Exits[i] = exitObject.transform;
            }
        }
    }
}
