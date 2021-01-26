using GameOn.SceneFieldProperty;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public class Tunnel : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Door[] doors;

        #endregion

        #region Properties

        public Door[] Doors => doors;
        public Dictionary<RoomContainer, Door> RoomsDoors { get; private set; } = new Dictionary<RoomContainer, Door>();

        #endregion

#if UNITY_EDITOR

        private void Reset()
        {
            UpdateReferences();
        }

        [ContextMenu("Update References")]
        private void UpdateReferences()
        {
            doors = GetComponentsInChildren<Door>();
            doors = doors.OrderBy(d => Vector3.Distance(d.transform.position, transform.position)).ToArray();

            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif

        public void CenterTriggered(Collider centerCollider, bool isEnter)
        {

        }
    }
}
