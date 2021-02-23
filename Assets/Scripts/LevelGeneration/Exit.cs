using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoguelikeVR.Interactions;

namespace RoguelikeVR.LevelGeneration
{
    public class Exit : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject[] exitBlockers;

        #endregion

        #region Properties

        public Door Door { get; private set; }

        #endregion

        public void Close()
        {
            if (exitBlockers.Length > 0)
            {
                var block = exitBlockers.Random();
                block.SetActive(true);
            }
            else
            {
                Debug.LogError("Exit has no block");
            }
        }
    }
}
