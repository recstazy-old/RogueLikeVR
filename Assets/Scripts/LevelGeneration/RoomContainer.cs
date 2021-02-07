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
        private SceneField scene;

        [SerializeField]
        private Room prefab;

        [SerializeField]
        private bool baked;

        #endregion

        #region Properties

        public SceneField RoomScene => scene;
        public Room Prefab => prefab;
        public bool Baked => baked;

        #endregion
    }
}
