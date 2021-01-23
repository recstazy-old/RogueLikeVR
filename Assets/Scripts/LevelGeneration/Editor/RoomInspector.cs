using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RoguelikeVR
{
    [CustomEditor(typeof(Room), true)]
    [CanEditMultipleObjects]
    public class RoomInspector : Editor
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var room = target as Room;

            GUILayout.Space(10);

            if (GUILayout.Button("Regenerate"))
            {
                room.RegenerateData();
                EditorUtility.SetDirty(room);
                EditorUtility.SetDirty(room.Bounds);
            }
        }
    }
}
