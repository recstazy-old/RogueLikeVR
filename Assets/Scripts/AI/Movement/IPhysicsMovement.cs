using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.AI
{
    public interface IPhysicsMovement
    {
        public bool IsActive { get; set; }
        void MoveBody(Rigidbody body, Transform currentTarget);
    }
}
