using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public interface IPhysicsLookAt
    {
        public bool IsActive { get; set; }
        public void MoveBodyRotation(Rigidbody body, Transform target);
    }
}
