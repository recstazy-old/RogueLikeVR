using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    [System.Serializable]
    public class RoomConnection
    {
        #region Fields

        public int OtherRoomIndex;
        public int ThisExitIndex;
        public int OtherExitIndex;
        public int TunnelIndex;
        public int TunnelDoorIndex;

        #endregion

        #region Properties
    
        #endregion
    }
}
