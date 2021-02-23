using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.Weapons
{
    public class ShootingWeapon : WeaponComponent
    {
        #region Fields

        [SerializeField]
        protected Transform shootPose;

        #endregion

        #region Properties

        #endregion

        protected override void StartedUsing()
        {
            TriggerPull();
        }

        protected override void StoppedUsing()
        {
            TriggerRelease();
        }

        protected virtual void TriggerPulled() { }
        protected virtual void TriggerReleased() { }

        private void TriggerPull()
        {
            TriggerPulled();
        }

        private void TriggerRelease()
        {
            TriggerReleased();
        }
    }
}
