using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace RoguelikeVR.AI
{
    public class ShootingRobotAI : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AIBase aiBase;

        [SerializeField]
        private TargetPoint testTargetPoint;

        [SerializeField]
        private Vector2 idleTimeRange;

        [SerializeField]
        private float randomRunRadius;

        [SerializeField]
        private Transform headPose;

        [SerializeField]
        private LayerMask obstacleMask = -1;

        [SerializeField]
        private Vector2 shootIdleRange;

        [SerializeField]
        private Vector2 timeBetweenShotsRange;

        [SerializeField]
        private Vector2Int shotsCountRange;

        private TargetPoint targetPoint;
        private Vector3 movementPosition;

        #endregion

        #region Properties
		
        #endregion

        [ContextMenu("Test")]
        public void Test()
        {
            SetShootingTarget(testTargetPoint);
        }

        public void SetShootingTarget(TargetPoint targetPoint)
        {
            this.targetPoint = targetPoint;
            StopAllCoroutines();
            StartCoroutine(MoveRoutine());
            StartCoroutine(ShootRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            while(targetPoint != null)
            {
                yield return null;
                aiBase.LookAt(targetPoint.transform);
                FindMovementTarget();
                aiBase.MoveTo(movementPosition);
                yield return new WaitForSeconds(Random.Range(idleTimeRange.x, idleTimeRange.y));
            }
        }

        private IEnumerator ShootRoutine()
        {
            while (targetPoint != null)
            {
                yield return new WaitForSeconds(Random.Range(shootIdleRange.x, shootIdleRange.y));
                int shotsCount = Random.Range(shotsCountRange.x, shotsCountRange.y + 1);

                for (int i = 0; i < shotsCount; i++)
                {
                    if (aiBase.WeaponHolder.Weapon == null) yield break;

                    aiBase.WeaponHolder.Weapon.StartUsingWeapon();
                    yield return null;
                    aiBase.WeaponHolder.Weapon.StopUsingWeapon();
                    yield return new WaitForSeconds(Random.Range(timeBetweenShotsRange.x, timeBetweenShotsRange.y));
                }
            }
        }

        private void FindMovementTarget()
        {
            var direction = targetPoint.transform.position - headPose.position;
            var ray = new Ray(headPose.position, direction.normalized);
            var hits = Physics.RaycastAll(ray, direction.magnitude, obstacleMask, QueryTriggerInteraction.Ignore);

            if (hits.Length > 0)
            {
                var closest = hits.OrderBy(h => h.distance).FirstOrDefault();
                movementPosition = closest.point;
            }
            else
            {
                Vector3 randomInCircle = Random.insideUnitCircle * randomRunRadius;
                randomInCircle.z = randomInCircle.y;
                randomInCircle.y = transform.position.y;
                movementPosition = transform.position + randomInCircle;
            }
        }
    }
}
