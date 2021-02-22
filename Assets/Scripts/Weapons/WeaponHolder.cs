using Recstazy.AniPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class WeaponHolder : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private BodyAttractor weaponAttractor;

        [SerializeField]
        private WeaponIK weaponIK;

        [SerializeField]
        private WeaponTargeting targeting;

        #endregion

        #region Properties

        #endregion

        private void Start()
        {
            SetWeapon(weapon);
        }

        public void SetWeapon(Weapon newWeapon)
        {
            weapon = newWeapon;
            weaponAttractor.SetAttachedBody(newWeapon?.MainBody);

            weaponIK.SetWeapon(newWeapon);
            weaponAttractor.Settings.Effector.SetEnabled(newWeapon != null);
            targeting.enabled = newWeapon != null;
        }

        public void ReleaseWeapon()
        {
            SetWeapon(null);
        }
    }
}
