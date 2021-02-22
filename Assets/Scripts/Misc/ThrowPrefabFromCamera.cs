using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class ThrowPrefabFromCamera : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Rigidbody prefab;

        [SerializeField]
        private float impulse;

        [SerializeField]
        private float mass = 30f;

        [SerializeField]
        private float destroyDelay;

        [SerializeField]
        private KeyCode key;

        #endregion

        #region Properties

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                if (prefab != null)
                {
                    var body = Instantiate(prefab, Camera.main.transform.position, Camera.main.transform.rotation, null);
                    body.mass = mass;
                    body.AddForce(Camera.main.transform.forward * impulse, ForceMode.VelocityChange);
                    Destroy(body.gameObject, destroyDelay);
                }
            }
        }
    }
}
