using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class AlignModelHipsWithRagdoll : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform transformToMove;

        [SerializeField]
        private Transform modelHips;

        [SerializeField]
        private Transform ragdollHips;

        [SerializeField]
        private Vector3Int alignAxesWeights = Vector3Int.up;

        #endregion

        #region Properties

        #endregion

        private void LateUpdate()
        {
            if (transformToMove != null && modelHips != null && ragdollHips != null)
            {
                var hipsDelta = ragdollHips.transform.position - modelHips.transform.position;
                var deltaWeighted = new Vector3(
                    hipsDelta.x * alignAxesWeights.x,
                    hipsDelta.y * alignAxesWeights.y,
                    hipsDelta.z * alignAxesWeights.z);

                transformToMove.position += deltaWeighted;
            }
        }
    }
}
