using RoguelikeVR.AI;
using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RoguelikeVR
{
    public class HeadIK : MonoBehaviour, ICharacterDependent
    {
        #region Fields

        [SerializeField]
        private float lookConeAngle;

        private TwoBoneIKConstraint ik;

        #endregion

        #region Properties

        public TargetPoint Target { get; set; }
        public CharacterDependencies Dependencies { get; set; }
        public int InitOrder => 0;

        #endregion

        private void Update()
        {
            if (ik != null)
            {
                if (Target != null)
                {
                    ik.weight = 1f;
                    var delta = Target.transform.position - ik.data.target.position;
                    var angle = Vector3.Angle(delta.normalized, transform.forward);

                    ik.data.target.position = Dependencies.Ragdoll.HeadCollider.transform.position;

                    if (angle < lookConeAngle)
                    {
                        ik.data.target.LookAt(Target.transform);
                    }
                }
                else
                {
                    ik.weight = 0f;
                }
            }
        }

        public void Initialized()
        {
            ik = Dependencies.HeadIK;
            Target = Dependencies.StartTargetPoint;
        }

        public void SetTargetPoint(TargetPoint targetPoint)
        {
            Target = targetPoint;
        }
    }
}
