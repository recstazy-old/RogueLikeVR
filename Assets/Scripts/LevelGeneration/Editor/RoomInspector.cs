using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameOn.UnityHelpers;
using UnityEditor.SceneManagement;

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

            if (GUILayout.Button("Bake lights"))
            {
                BakeLights();
            }
        }

        private void BakeLights()
        {
            var room = target as Room;
            var statics = room.GetComponentsInChildren<MakeStaticOnBake>();

            foreach (var s in statics)
            {
                s.SetIsStatic(true);
            }

            bool started = Lightmapping.BakeAsync();

            if (started)
            {
                Lightmapping.bakeCompleted += LightmappingComplete;
            }
            else
            {
                foreach (var s in statics)
                {
                    s.SetIsStatic(false);
                }
            }
        }

        private void LightmappingComplete()
        {
            Lightmapping.bakeCompleted -= LightmappingComplete;

            var room = target as Room;
            var statics = room.GetComponentsInChildren<MakeStaticOnBake>();

            foreach (var s in statics)
            {
                s.SetIsStatic(false);
            }
        }
    }
}
