using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class GameConfig : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private LevelGenerator levelGenerator;

        [SerializeField]
        private SpawnPoint playerSpawn;

        [SerializeField]
        private RoomLoader roomLoader;

        #endregion

        #region Properties

        public LevelGenerator LevelGenerator { get => levelGenerator; }
        public SpawnPoint PlayerSpawn { get => playerSpawn; }
        public RoomLoader RoomLoader => roomLoader;

        #endregion
    }
}
