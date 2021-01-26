using GameOn.EasingUtils;
using GameOn.UnityHelpers;
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

        public bool Opened { get; private set; }
        public bool Locked { get; private set; }

        #endregion

#if UNITY_EDITOR

        private void Reset()
        {
            doorAnimation = GetComponentInChildren<EasingExecutable>();
            UnityEditor.EditorUtility.SetDirty(this);
        }

#endif

        public void SetOpened(bool isOpened)
        {
            if (isOpened)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Open()
        {
            if (!Locked && !Opened)
            {
                doorAnimation.TryExecute(true);

                if (doorAnimation.IsRunning)
                {
                    this.RunDelayed(doorAnimation.EasingTime, () => Opened = true);
                }
            }
        }

        public void Close()
        {
            if (!Locked && Opened)
            {
                doorAnimation.TryExecute(false);

                if (doorAnimation.IsRunning)
                {
                    this.RunDelayed(doorAnimation.EasingTime, () => Opened = false);
                }
            }
        }

        public void ForceLockOnState(bool isOpened)
        {
            SetOpened(isOpened);
            Locked = true;
        }

        public void Unlock()
        {
            Locked = false;
        }
    }
}
