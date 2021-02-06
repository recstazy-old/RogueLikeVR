using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public interface IPhysicsMovement
    {
        void MoveBody(Rigidbody body, Transform currentTarget);
    }
}
