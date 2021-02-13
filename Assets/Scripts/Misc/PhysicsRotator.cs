using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class PhysicsRotator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Vector3 axis;

        [SerializeField]
        private float speed;

        #endregion

        #region Properties

        #endregion

        private void FixedUpdate()
        {
            transform.Rotate(axis, speed * Time.fixedDeltaTime);
        }
    }
}
