using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class WeaponComponent : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        public void StartUsing()
        {
            StartedUsing();
        }

        public void StopUsing()
        {
            StoppedUsing();
        }

        protected virtual void StartedUsing() { }
        protected virtual void StoppedUsing() { }
    }
}
