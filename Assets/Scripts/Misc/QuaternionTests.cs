using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class QuaternionTests : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform from;

        [SerializeField]
        private Transform to;

        [SerializeField]
        private Transform target;

        #endregion

        #region Properties

        #endregion

        private IEnumerator Start()
        {
            while(true)
            {
                yield return null;
                Rotate();
            }
        }

        private void Rotate()
        {
            target.transform.rotation = Quaternion.RotateTowards(target.rotation, to.rotation, 1440f);
        }
    }
}
