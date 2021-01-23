using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameOn.SceneFieldProperty;

namespace RoguelikeVR
{
    [CreateAssetMenu(fileName = "NewRoom", menuName = "Level Generation/New Room", order = 131)]
    public class RoomContainer : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private SceneField room;

        #endregion

        #region Properties

        public SceneField RoomScene => room;

        #endregion
    }
}
