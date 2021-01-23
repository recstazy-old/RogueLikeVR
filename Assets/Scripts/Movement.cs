using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class Movement : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private CharacterController body;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float lerpSpeed;

        private Vector3 movement;
        private Vector3 currentVelocity;

        #endregion

        #region Properties

        #endregion

        private void Update()
        {
            movement = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movement += speed * body.transform.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement += -speed * body.transform.forward;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movement += speed * body.transform.right;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement += -speed * body.transform.right;
            }

            currentVelocity = Vector3.Lerp(currentVelocity, movement, Time.deltaTime * lerpSpeed);
            body.SimpleMove(currentVelocity);
        }
    }

}
