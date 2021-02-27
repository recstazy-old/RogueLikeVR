using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public class DriveAnimator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private DriveBlend globalBlend;

        #endregion

        #region Properties

        public DriveBlend GlobalBlend { get => globalBlend; set => globalBlend = value; }

        #endregion

        private void Awake()
        {
            CheckBlendValid();
            var drivers = GetComponentsInChildren<DriveReference>();

            foreach (var d in drivers)
            {
                d.GlobalBlend = globalBlend;
            }
        }

        private void CheckBlendValid()
        {
            if (globalBlend == null)
            {
                var blend = GetComponent<DriveBlend>();

                if (blend == null)
                {
                    blend = gameObject.AddComponent<DriveBlend>();
                }

                globalBlend = blend;
            }
        }
    }
}
