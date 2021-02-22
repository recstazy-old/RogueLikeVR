using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.AniPhysics
{
    public class PhysicsAnimationBlender : MonoBehaviour
    {
        [System.Serializable]
        public class TransformTouple
        {
            public Transform Reference;
            public Transform Target;
        }

        #region Fields

        [SerializeField]
        private Transform referenceRootBone;

        [SerializeField]
        private Transform targetRootBone;

        [SerializeField]
        private StabJointSettingsAsset defaultSettings;

        [SerializeField]
        private StabEffector effectBlend;

        [SerializeField]
        private TransformTouple[] touples;

        #endregion

        #region Properties

        public StabJointSettingsAsset Settings => defaultSettings;
        public List<Rigidbody> Bodies { get; private set; } = new List<Rigidbody>();
        public StabEffector Effector => effectBlend;
        public TransformTouple[] Touples { get => touples; set => touples = value; }
        public Transform ReferenceRootBone { get => referenceRootBone; }
        public Transform TargetRootBone { get => targetRootBone; }

        #endregion

        private void Start()
        {
            SetupBodies(touples);
        }

#if UNITY_EDITOR

        [ContextMenu("ExecuteSetupInEditor")]
        public void ExecuteSetupInEditor()
        {
            SetupBodies(touples);

            foreach (var t in touples)
            {
                UnityEditor.EditorUtility.SetDirty(t.Reference.gameObject);
                UnityEditor.EditorUtility.SetDirty(t.Target.gameObject);
            }
        }

#endif

        private void SetupBodies(TransformTouple[] touples)
        {
            foreach (var t in touples)
            {
                var stab = t.Reference.GetComponent<BoneAttractor>();

                if (stab == null)
                {
                    stab = t.Reference.gameObject.AddComponent<BoneAttractor>();
                }

                var body = t.Target.GetComponent<Rigidbody>();

                if (body == null)
                {
                    body = t.Target.gameObject.AddComponent<Rigidbody>();
                }

                Bodies.Add(body);

                if (stab.ConnectedBody == null)
                {
                    stab.SetAttachedBody(body);
                    stab.Settings = new StabSettings(defaultSettings.Settings);
                }

                stab.Settings.Effector = effectBlend;
            }
        }
    }
}
