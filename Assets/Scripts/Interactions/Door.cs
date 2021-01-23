using GameOn.EasingUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class Door : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private EasingExecutable doorAnimation;

        #endregion

        #region Properties

        #endregion

#if UNITY_EDITOR

        private void Reset()
        {
            doorAnimation = GetComponentInChildren<EasingExecutable>();
            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif

        public void Open()
        {
            doorAnimation.TryExecute(true);
        }

        public void Close()
        {
            doorAnimation.TryExecute(false);
        }
    }
}
