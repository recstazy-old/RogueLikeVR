using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace RoguelikeVR
{
    public class CreateReferenceModel : ScriptableWizard
    {
        #region Fields

        [SerializeField]
        private Transform root;

        #endregion

        #region Properties

        #endregion

        [MenuItem("Tools/Custom Wizards/Create reference model wizard")]
        public static void OpenWizard()
        {
            ScriptableWizard.DisplayWizard<CreateReferenceModel>("Reference creator", "Create");
        }

        private void OnWizardCreate()
        {
            if (root != null)
            {
                var refRoot = Instantiate(root.gameObject, root.transform.position + Vector3.forward, root.rotation, root.parent).transform;

                Component[] physicsComponents = refRoot.GetComponentsInChildren<Joint>(true).Select(b => b as Component)
                    .Concat(refRoot.GetComponentsInChildren<Rigidbody>(true))
                    .Concat(refRoot.GetComponentsInChildren<Collider>())
                    .ToArray();

                foreach (var c in physicsComponents)
                {
                    var componentGameobject = c.gameObject;
                    DestroyImmediate(c);
                    EditorUtility.SetDirty(componentGameobject);
                }

                var rootJoints = root.GetComponentsInChildren<ConfigurableJoint>();

                foreach (var j in rootJoints)
                {
                    var path = j.transform.GetBoneRelativePath(root);
                    var referenceBone = refRoot.Find(path);

                    if (referenceBone != null)
                    {
                        var motorCopy = referenceBone.gameObject.AddComponent<CopyBoneToMotor>();
                        motorCopy.Joint = j;
                        motorCopy.Reference = referenceBone;
                        EditorUtility.SetDirty(referenceBone.gameObject);
                    }

                    EditorUtility.SetDirty(refRoot);
                }
            }
        }
    }
}
