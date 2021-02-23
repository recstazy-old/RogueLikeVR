using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace RoguelikeVR
{
    public class CopyRagdollWizard : ScriptableWizard
    {
        #region Fields

        [SerializeField]
        private Transform sourceRoot;

        [SerializeField]
        private Transform targetRoot;

        #endregion

        #region Properties
		
        #endregion

        [MenuItem("Tools/Custom Wizards/Ragdoll Copy Wizard")]
        public static void OpenWizard()
        {
            ScriptableWizard.DisplayWizard<CopyRagdollWizard>("Copy Ragdoll Wizard", "Copy ragdoll physics");
        }

        private void OnWizardCreate()
        {
            if (sourceRoot != null && targetRoot != null)
            {
                var components = sourceRoot.GetComponentsInChildren<Rigidbody>().Select(b => b as Component)
                    .Concat(sourceRoot.GetComponentsInChildren<Collider>())
                    .Concat(sourceRoot.GetComponentsInChildren<Joint>())
                    .ToArray();
                    
                foreach (var c in components)
                {
                    string path = c.transform.GetBoneRelativePath(sourceRoot);
                    var target = targetRoot.Find(path);

                    if (target != null)
                    {
                        var newComponent = target.gameObject.AddComponent(c.GetType());
                        ComponentUtility.CopyComponent(c);
                        ComponentUtility.PasteComponentValues(newComponent);

                        if (newComponent is Joint)
                        {
                            var newJoint = newComponent as Joint;
                            var connectedBodyPath = (c as Joint).connectedBody.transform.GetBoneRelativePath(sourceRoot);
                            var bodyTransform = targetRoot.Find(connectedBodyPath);
                            newJoint.connectedBody = bodyTransform ? bodyTransform.GetComponent<Rigidbody>() : null;

                            EditorUtility.SetDirty(newJoint);
                        }
                    }
                }
            }
        }
    }
}
