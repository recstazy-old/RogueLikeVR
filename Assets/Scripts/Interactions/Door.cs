using GameOn.EasingUtils;
using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeVR
{
    public class Door : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private EasingExecutable doorAnimation;

        [SerializeField]
        private UnityEvent onPendingOpen;

        [SerializeField]
        private UnityEvent onOpened;

        [SerializeField]
        private UnityEvent onPendingClose;

        [SerializeField]
        private UnityEvent onClosed;

        #endregion

        #region Properties

        public bool Opened { get; private set; }
        public bool Locked { get; private set; }
        public bool IsInTransition => doorAnimation.IsRunning;

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
            if (!Locked && !Opened && !IsInTransition)
            {
                doorAnimation.TryExecute(true);

                if (IsInTransition)
                {
                    onPendingOpen?.Invoke();

                    this.RunDelayed(doorAnimation.EasingTime, () => 
                    { 
                        Opened = true;
                        onOpened?.Invoke();
                    });
                }
            }
        }

        public void Close()
        {
            if (!Locked && Opened && !IsInTransition)
            {
                doorAnimation.TryExecute(false);

                if (IsInTransition)
                {
                    onPendingClose?.Invoke();

                    this.RunDelayed(doorAnimation.EasingTime, () => 
                    { 
                        Opened = false;
                        onClosed?.Invoke();
                    });
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
