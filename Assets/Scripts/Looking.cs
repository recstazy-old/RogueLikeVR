using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class Looking : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private CharacterController body;

        [SerializeField]
        private Transform head;

        [SerializeField]
        private Vector2 sensivity;

        private float pitch = 0.5f;
        private Quaternion downRotation;
        private Quaternion upRotation;

        private Vector2 deltaMouse;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            downRotation = Quaternion.LookRotation(Vector3.up, Vector3.back);
            upRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                deltaMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) / Time.deltaTime;
            }
            else
            {
                deltaMouse = Vector2.zero;
            }

            if (head != null)
            {
                pitch += sensivity.y * deltaMouse.y * 0.01f;
                pitch = Mathf.Clamp01(pitch);
                head.localRotation = Quaternion.Lerp(downRotation, upRotation, pitch);
            }

            if (body != null)
            {
                body.transform.eulerAngles = body.transform.eulerAngles + Vector3.up * deltaMouse.x * sensivity.x * Time.deltaTime;
            }
        }
    }
}
