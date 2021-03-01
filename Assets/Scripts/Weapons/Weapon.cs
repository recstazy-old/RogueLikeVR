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

        [SerializeField]
        private WeaponComponent implementation;

        private Rigidbody mainBody;

        #endregion

        #region Properties

        public Grip MainGrip { get => mainGripPoint; }
        public Grip[] SecondaryGripPoints { get => secondaryGripPoints; }
        public Rigidbody MainBody
        {
            get
            {
                if (mainBody == null)
                {
                    mainBody = GetComponent<Rigidbody>();
                }

                return mainBody;
            }
        }

        #endregion

        public void StartUsingWeapon()
        {
            implementation?.StartUsing();
        }

        public void StopUsingWeapon()
        {
            implementation?.StopUsing();
        }
    }
}
