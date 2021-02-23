using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using RoguelikeVR;

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

        [MenuItem("Tools/Custom Wizards/Open Blender Wizard")]
        public static void OpenWindw()
        {
            ScriptableWizard.DisplayWizard<AnimationBlenderWizard>("Physics animation wizard", "Create Bindings");
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

                blender.ExecuteSetupInEditor();
            }
        }

        private Transform FindReferenceBone(Transform target)
        {
            if (blender != null)
            {
                var targetPath = target.GetBoneRelativePath(blender.TargetRootBone);

                if (targetPath != null)
                {
                    var referenceBone = blender.ReferenceRootBone.Find(targetPath);
                    return referenceBone;
                }
            }

            return null;
        }

        private void OnWizardUpdate()
        {
            helpString = "Select bones with Rigidbodies in ragdoll which you want to match with animation";
        }
    }
}
