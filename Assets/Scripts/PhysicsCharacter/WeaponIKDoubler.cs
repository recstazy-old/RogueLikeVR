using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class WeaponIKDoubler : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform mainGripPoint;

        [SerializeField]
        private bool createGhost;

        #endregion

        #region Properties

        public Transform MainGripPoint => mainGripPoint;

        #endregion

        public void Initialize(Weapon weapon, Transform mainGripReferencePoint)
        {
            SetupTransforms(weapon, mainGripReferencePoint);
        }

        private void SetupTransforms(Weapon weapon, Transform mainGripReference)
        {
            transform.SetPositionAndRotation(weapon.transform.position, weapon.transform.rotation);
            mainGripPoint.transform.SetPositionAndRotation(weapon.MainGrip.transform.position, weapon.MainGrip.transform.rotation);

            transform.rotation = mainGripReference.transform.rotation;
            var deltaPosition = mainGripReference.transform.position - mainGripPoint.transform.position;
            transform.position += deltaPosition;

            if (createGhost)
            {
                CreateTestVisual(weapon);
            }
        }

        private void CreateTestVisual(Weapon weapon)
        {
            var visual = weapon.transform.GetChild(0).gameObject;
            var newVisual = Instantiate(visual);

            var colliders = newVisual.GetComponentsInChildren<Collider>();

            foreach (var c in colliders)
            {
                Destroy(c);
            }

            newVisual.transform.SetParent(transform);
            newVisual.transform.localPosition = visual.transform.localPosition;
            newVisual.transform.localRotation = visual.transform.localRotation;

            var renderers = newVisual.GetComponentsInChildren<MeshRenderer>();
            var material = Resources.Load<Material>("Ghost");

            foreach (var renderer in renderers)
            {
                renderer.material = material;
            }
        }
    }
}
