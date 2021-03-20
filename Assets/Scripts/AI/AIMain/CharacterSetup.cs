using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations.Rigging;

namespace RoguelikeVR.AI
{
    public class CharacterSetup : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private CharacterDependencies dependencies;

        #endregion

        #region Properties

        public float RagdollAnimationWeight { get => Dependencies.Ragdoll ? Dependencies.Ragdoll.GlobalBlendAmount : 1f; set => Dependencies.Ragdoll?.SetGlobalBlend(value); }
        public CharacterDependencies Dependencies { get => dependencies; }

        #endregion

        private void Awake()
        {
            var dependents = GetComponentsInChildren<ICharacterDependent>().OrderBy(d => d.InitOrder).ToArray();

            foreach (var d in dependents)
            {
                d.Dependencies = Dependencies;
                d.Initialized();
            }

            if (Dependencies.MovementBody != null)
            {
                transform.SetParent(Dependencies.MovementBody.transform);
            }
        }
    }
}
