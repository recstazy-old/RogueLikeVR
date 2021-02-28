using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Animations;
using RoguelikeVR.PhysicsCharacters;

namespace RoguelikeVR
{
    public class CreateReferenceModel : ScriptableWizard
    {
        #region Fields

        [SerializeField]
        private Transform ragdollRoot;

        [SerializeField]
        private AnimatorController animatorToAttach;

        [SerializeField]
        private Material replaceMaterial;

        [SerializeField]
        private Rigidbody referenceRootPrefab;

        #endregion

        #region Properties

        #endregion

        [MenuItem("Tools/Custom Wizards/Create reference model wizard")]
        public static void OpenWizard()
        {
            var window = ScriptableWizard.DisplayWizard<CreateReferenceModel>("Reference creator", "Create");

            if (Selection.activeTransform != null)
            {
                window.ragdollRoot = Selection.activeTransform;
            }
        }

        private void OnWizardCreate()
        {
            if (ragdollRoot != null)
            {
                var refRoot = CreateReferenceRoot();
                CreateDriveReferences(refRoot);
                ReplaceMaterial(refRoot);
                Animator animator = CreateAnimator(refRoot);
                var driveAnimator = CreateDriveAnimator(refRoot);
                CreateParentAndLocomotion(refRoot, animator);
                CreateRootParent(refRoot);
            }
        }

        private Transform CreateReferenceRoot()
        {
            var refRoot = Instantiate(ragdollRoot.gameObject, ragdollRoot.transform.position, ragdollRoot.rotation, ragdollRoot.parent).transform;
            refRoot.name = ragdollRoot.name + "_Reference";

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

            var customBehaviours = refRoot.GetComponentsInChildren<DriveBlend>()
                .Select(b => b as MonoBehaviour)
                .Concat(new MonoBehaviour[] { refRoot.GetComponentInChildren<RagdollCharacter>() })
                .Concat(new MonoBehaviour[] { refRoot.GetComponentInChildren<BodyAngularBrake>() });

            foreach (var b in customBehaviours)
            {
                if (b != null)
                {
                    var gObject = b.gameObject;
                    DestroyImmediate(b);
                    EditorUtility.SetDirty(gObject);
                }
            }

            var blendGroup = refRoot.GetComponentInChildren<BlendGroup>();

            if (blendGroup != null)
            {
                var parent = blendGroup.transform.parent.parent;
                DestroyImmediate(blendGroup.transform.parent.gameObject);
                EditorUtility.SetDirty(parent);
            }

            var ignoreCollisions = refRoot.GetComponentsInChildren<IgnoreCollisionWithGameObject>();

            foreach (var i in ignoreCollisions)
            {
                var gObject = i.gameObject;
                DestroyImmediate(i);
                EditorUtility.SetDirty(gObject);
            }

            return refRoot;
        }

        private void CreateDriveReferences(Transform refRoot)
        {
            var rootJoints = ragdollRoot.GetComponentsInChildren<ConfigurableJoint>();

            foreach (var j in rootJoints)
            {
                var path = j.transform.GetBoneRelativePath(ragdollRoot);
                var referenceBone = refRoot.Find(path);

                if (referenceBone != null)
                {
                    var driver = referenceBone.gameObject.AddComponent<DriveReference>();
                    driver.Joint = j;
                    driver.Reference = referenceBone;

                    if (driver.gameObject.name.ToLower().Contains("hips"))
                    {
                        driver.CopyPosition = true;
                    }

                    EditorUtility.SetDirty(referenceBone.gameObject);
                }

                EditorUtility.SetDirty(refRoot);
            }
        }

        private void ReplaceMaterial(Transform refRoot)
        {
            if (replaceMaterial != null)
            {
                var renderer = refRoot.GetComponentInChildren<SkinnedMeshRenderer>();

                if (renderer != null)
                {
                    var materials = renderer.sharedMaterials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i] = replaceMaterial;
                    }

                    renderer.sharedMaterials = materials;
                    EditorUtility.SetDirty(renderer);
                }
            }
        }

        private Animator CreateAnimator(Transform refRoot)
        {
            if (animatorToAttach != null)
            {
                var animator = refRoot.gameObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorToAttach;
                animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
                EditorUtility.SetDirty(animator);

                return animator;
            }

            return null;
        }

        private DriveAnimator CreateDriveAnimator(Transform refRoot)
        {
            var driveAnimator = refRoot.gameObject.AddComponent<DriveAnimator>();
            EditorUtility.SetDirty(driveAnimator);
            return driveAnimator;
        }

        private void CreateParentAndLocomotion(Transform refRoot, Animator animator)
        {
            var newRootInstance = Instantiate(referenceRootPrefab, refRoot.position, refRoot.rotation, refRoot.parent);
            refRoot.transform.SetParent(newRootInstance.transform);
            var velocityToAnimtor = newRootInstance.GetComponentInChildren<BodyVelocityToAnimator>();
            velocityToAnimtor.Animator = animator;

            var joint = newRootInstance.GetComponent<Joint>();
            joint.connectedBody = ragdollRoot.GetComponent<Rigidbody>();

            EditorUtility.SetDirty(newRootInstance.gameObject);
            EditorUtility.SetDirty(joint);
            EditorUtility.SetDirty(refRoot);
            EditorUtility.SetDirty(velocityToAnimtor);
        }

        private void CreateRootParent(Transform refRoot)
        {
            var rootParent = new GameObject(ragdollRoot.name + "_Root");
            rootParent.transform.SetPositionAndRotation(ragdollRoot.transform.position, ragdollRoot.transform.rotation);

            ragdollRoot.SetParent(rootParent.transform);
            refRoot.parent.SetParent(rootParent.transform);

            EditorUtility.SetDirty(rootParent);
            EditorUtility.SetDirty(ragdollRoot.gameObject);
            EditorUtility.SetDirty(refRoot.gameObject);
        }
    }
}
