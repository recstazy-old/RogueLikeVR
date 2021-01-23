using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class GameController : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameConfig gameConfig;

        #endregion

        #region Properties

        #endregion

        private void Start()
        {
            gameConfig.LevelGenerator.Generate(LevelGenerationFinished);
        }

        private void LevelGenerationFinished()
        {
            gameConfig.PlayerSpawn.Spawn();
        }
    }
}
