using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class LevelGenerator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameConfig gameConfig;

        [SerializeField]
        private RoomVariantContainer roomsContainer;

        [SerializeField]
        private int roomsCount;

        private System.Action onFinished;

        #endregion

        #region Properties

        public List<Room> Rooms { get; private set; } = new List<Room>();

        #endregion

        public void Generate(System.Action onFinished)
        {
            this.onFinished = onFinished;
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            var room = roomsContainer.Variants.Random();
            gameConfig.RoomLoader.Load(room, FinishedLoading);
        }

        private void FinishedLoading(Room room)
        {
            onFinished?.Invoke();
        }
    }
}
