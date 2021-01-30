using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class RoomConvexGroup : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private BoxCollider box;

        [SerializeField]
        private bool custom;

        #endregion

        #region Properties

        public BoxCollider Box => box;
        public bool IsValidBounds { get; private set; }

        #endregion

        public void UpdateBoxAndSetDirty()
        {
            UpdateBox();

#if UNITY_EDITOR

            UnityEditor.EditorUtility.SetDirty(Box.gameObject);
            UnityEditor.EditorUtility.SetDirty(Box);
            UnityEditor.EditorUtility.SetDirty(this);

#endif
        }

        public void UpdateBox()
        {
            if (custom)
            {
                return;
            }

            if (box == null)
            {
                var boxObject = new GameObject(gameObject.name + "_Bounds");
                box = boxObject.AddComponent<BoxCollider>();
                box.transform.SetParent(transform);
                box.transform.SetAsFirstSibling();
                box.transform.localPosition = Vector3.zero;
            }

            var bounds = gameObject.EncapsulateAllChildren();
            box.transform.position = bounds.center;
            box.size = bounds.size;
            IsValidBounds = bounds.center != Vector3Int.zero && bounds.size != Vector3Int.zero;
        }
    }
}
