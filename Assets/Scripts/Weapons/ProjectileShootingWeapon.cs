using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.Weapons
{
    public class ProjectileShootingWeapon : ShootingWeapon
    {
        #region Fields

        [SerializeField]
        private Projectile projectilePrefab;

        [SerializeField]
        private float impulse;

        #endregion

        #region Properties

        #endregion

        protected override void TriggerPulled()
        {
            ShootProjectile();
        }

        private void ShootProjectile()
        {
            var instance = Instantiate(projectilePrefab, shootPose.position, shootPose.rotation, null);
            instance.Body.AddForce(impulse * shootPose.forward, ForceMode.VelocityChange);
            instance.ShotMade();
        }
    }
}
