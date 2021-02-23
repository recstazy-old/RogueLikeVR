using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.Weapons
{
    public class Projectile : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private float lifeTime;

        #endregion

        #region Properties

        public Rigidbody Body => body;

        #endregion

        public void ShotMade()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
