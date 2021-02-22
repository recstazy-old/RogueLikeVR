using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameOn.UnityHelpers;
using System.Linq;

namespace Recstazy.AniPhysics
{
    public class AnimationBlenderWizard : ScriptableWizard
    {
        #region Fields

        [SerializeField]
        private PhysicsAnimationBlender blender;

        #endregion

        #region Properties
		
        #endregion

        [MenuItem("Tools/Open Blender Wizard")]
        public static void OpenWindw()
        {
            ScriptableWizard.DisplayWizard<AnimationBlenderWizard>("Physics animation wizard", "Create Bindings", "Create Components");
        }

        private void OnWizardCreate()
        {
            if (blender != null)
            {
                var touples = new List<PhysicsAnimationBlender.TransformTouple>();
                var selectedBodies = Selection.gameObjects.Select(o => o.GetComponent<Rigidbody>()).Where(b => b != null).ToArray();

                foreach (var s in selectedBodies)
                {
                    var reference = FindReferenceBone(s.transform);

                    if (reference != null)
                    {
                        touples.Add(new PhysicsAnimationBlender.TransformTouple() { Reference = reference, Target = s.transform });
                    }
                }

                blender.Touples = touples.ToArray();
                EditorUtility.SetDirty(blender);
            }
        }

        private void OnWizardOtherButton()
        {
            if (blender != null)
            {
                blender.ExecuteSetupInEditor();
            }
        }

        private Transform FindReferenceBone(Transform target)
        {
            if (blender != null)
            {
                var targetPath = GetBoneRelativeToRootPath(target, blender.TargetRootBone);

                if (targetPath != null)
                {
                    var referenceBone = blender.ReferenceRootBone.Find(targetPath);
                    return referenceBone;
                }
            }

            return null;
        }

        private string GetBoneRelativeToRootPath(Transform bone, Transform root)
        {
            if (blender == null)
            {
                return null;
            }

            string result = bone.name;
            var parent = bone.parent;
            int i = 0;

            while (parent != null && parent != root && i < 100)
            {
                result = parent.name + "/" + result;
                parent = parent.parent;
                i++;
            }

            return result;
        }
    }
}
