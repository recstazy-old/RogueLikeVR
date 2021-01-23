using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class SpawnPoint : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private Transform parent;

        #endregion

        #region Properties
        
        #endregion

        public GameObject Spawn()
        {
            return Instantiate(prefab, transform.position, transform.rotation, parent);
        }

        public GameObject Spawn(Transform parent)
        {
            this.parent = parent;
            return Spawn();
        }
    }
}
