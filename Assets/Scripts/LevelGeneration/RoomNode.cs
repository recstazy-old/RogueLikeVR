using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    [System.Serializable]
    public class RoomNode
    {
        #region Fields

        public string Name;
        public int ThisRoomIndex;
        public Transform transform => Holder.transform;
        public int ExitCount;
        public List<int> OpenExits = new List<int>();
        public List<int> ConnectedExits = new List<int>();
        public List<RoomConnection> Connections = new List<RoomConnection>();
        public RoomPlaceholder Holder;

        #endregion

        #region Properties
    
        #endregion

        public void FillOpen()
        {
            for (int i = 0; i < ExitCount; i++)
            {
                OpenExits.Add(i);
            }
        }
    }
}
