using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoguelikeVR.Main
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void LevelGenerationFinished()
        {
            gameConfig.PlayerSpawn.Spawn();
        }
    }
}
