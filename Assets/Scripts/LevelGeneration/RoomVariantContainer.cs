using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.LevelGeneration
{
    [CreateAssetMenu(fileName = "NewRoomVariantContainer", menuName = "Level Generation/New Room Variant Container", order = 131)]
    public class RoomVariantContainer : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private RoomContainer[] roomVariants;

        #endregion

        #region Properties

        public RoomContainer[] Variants => roomVariants;

        #endregion
    }
}
