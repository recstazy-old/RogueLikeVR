using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR.AI
{
    public class ShootingRobotAI : MonoBehaviour, ITargetPointReciever
    {
        #region Fields

        [SerializeField]
        private AIBase aiBase;

        [SerializeField]
        private Vector2 idleTimeRange;

        [SerializeField]
        private Vector2 randomRunRadiusRange;

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
		
        public bool IsActive { get; private set; }

        #endregion

        public void SetTargetPoint(TargetPoint point)
        {
            SetShootingTarget(point);
        }

        public void SetShootingTarget(TargetPoint targetPoint)
        {
            this.targetPoint = targetPoint;
            aiBase.WeaponHolder.WeaponTargeter.TargetPoint = targetPoint;
            SetActive(true);
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;

            if (isActive)
            {
                if (targetPoint != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(MoveRoutine());

                    var lookTransform = targetPoint ? targetPoint.transform : aiBase.Movement.NavAgent.transform;
                    aiBase.LookAt(lookTransform);

                    if (aiBase.WeaponHolder.Weapon != null)
                    {
                        StartCoroutine(ShootRoutine());
                    }
                }
            }
            else
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator MoveRoutine()
        {
            while(targetPoint != null)
            {
                yield return null;
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
            Vector3 randomInCircle = Random.onUnitSphere * Random.Range(randomRunRadiusRange.x, randomRunRadiusRange.y);
            randomInCircle.y = 0f;
            movementPosition = transform.position + randomInCircle;
        }
    }
}
