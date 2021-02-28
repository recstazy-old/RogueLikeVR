using Recstazy.AniPhysics;
using RoguelikeVR.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.Weapons
{
    public class WeaponHolder : MonoBehaviour, IWeaponReciever
    {
        #region Fields

        [SerializeField]
        private Weapon weapon;

        [SerializeField]
        private WeaponIK weaponIK;

        [SerializeField]
        private WeaponTargeting targeting;

        #endregion

        #region Properties

        public Weapon Weapon => weapon;
        public WeaponTargeting WeaponTargeter => targeting;

        #endregion

        public void SetWeapon(Weapon newWeapon)
        {
            weapon = newWeapon;
            weaponIK.SetWeapon(newWeapon);
            targeting.enabled = newWeapon != null;
        }

        public void ReleaseWeapon()
        {
            SetWeapon(null);
        }
    }
}
