using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RoguelikeVR
{
    public class ReplaceCharaterJointsWithConfigurable : ScriptableWizard
    {
        #region Fields

        [SerializeField]
        private Transform root;

        #endregion

        #region Properties
		
        #endregion

        [MenuItem("Tools/Custom Wizards/Replace character with configurable wizard")]
        public static void OpenWizard()
        {
            ScriptableWizard.DisplayWizard<ReplaceCharaterJointsWithConfigurable>("Replacer", "Replace");
        }

        private void OnWizardCreate()
        {
            if (root != null)
            {
                var joints = root.GetComponentsInChildren<CharacterJoint>();

                foreach (var j in joints)
                {
                    var newJoint = j.gameObject.AddComponent<ConfigurableJoint>();

                    newJoint.connectedBody = j.connectedBody;
                    newJoint.axis = j.axis;
                    newJoint.secondaryAxis = j.swingAxis;
                    newJoint.anchor = j.anchor;

                    newJoint.xMotion = newJoint.yMotion = newJoint.zMotion = ConfigurableJointMotion.Locked;
                    newJoint.angularXMotion = newJoint.angularYMotion = newJoint.angularZMotion = ConfigurableJointMotion.Limited;

                    newJoint.lowAngularXLimit = j.lowTwistLimit;
                    newJoint.highAngularXLimit = j.highTwistLimit;
                    newJoint.angularYLimit = j.swing1Limit;
                    newJoint.angularZLimit = j.swing2Limit;

                    DestroyImmediate(j);
                    EditorUtility.SetDirty(newJoint);
                    EditorUtility.SetDirty(newJoint.gameObject);
                }
            }
        }
    }
}