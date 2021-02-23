using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoguelikeVR.Interactions;

namespace RoguelikeVR.Weapons
{
    public class Weapon : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Grip mainGripPoint;

        [SerializeField]
        private Grip[] secondaryGripPoints;

        #endregion

        #region Properties

        public Grip MainGrip { get => mainGripPoint; }
        public Grip[] SecondaryGripPoints { get => secondaryGripPoints; }
        public Rigidbody MainBody { get; private set; }

        #endregion

        private void Awake()
        {
            MainBody = GetComponent<Rigidbody>();
        }
    }
}
