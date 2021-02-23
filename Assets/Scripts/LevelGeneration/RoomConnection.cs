using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.LevelGeneration
{
    [System.Serializable]
    public struct RoomConnection
    {
        #region Fields

        public int ThisRoomIndex;
        public int OtherRoomIndex;
        public int ThisExitIndex;
        public int OtherExitIndex;

        #endregion

        #region Properties
    
        #endregion

        public bool IsInvertOf(RoomConnection other)
        {
            other.Invert();
            return Equal(other);
        }

        public void Invert()
        {
            int temp = ThisRoomIndex;
            ThisRoomIndex = OtherRoomIndex;
            OtherRoomIndex = temp;

            temp = ThisExitIndex;
            ThisExitIndex = OtherExitIndex;
            OtherExitIndex = temp;
        }

        public bool Equal(RoomConnection other)
        {
            return ThisRoomIndex == other.ThisRoomIndex 
                && OtherRoomIndex == other.OtherRoomIndex 
                && ThisExitIndex == other.ThisExitIndex 
                && OtherExitIndex == other.OtherExitIndex;
        }
    }
}
