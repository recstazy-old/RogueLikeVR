using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public static class Extensions
    {
        public static int RandomIndex<T>(this IEnumerable<T> enumerable)
        {
            return Random.Range(0, enumerable.Count());
        }

        /// <summary>
        /// Works if collider is rotated only around Y axis
        /// </summary>
        public static Bounds GetWorldBoundsInXZPlane(this BoxCollider box)
        {
            Vector3 min = box.center - box.size * 0.5f;
            Vector3 max = box.center + box.size * 0.5f;

            Vector3 minWorld = box.transform.TransformPoint(min);
            Vector3 maxWorld = box.transform.TransformPoint(max);
            Vector3 minMaxFront = box.transform.TransformPoint(new Vector3(max.x, min.y, min.z));
            Vector3 minMaxBack = box.transform.TransformPoint(new Vector3(min.x, max.y, max.z));

            var bounds = new Bounds(minWorld, Vector3.zero);
            bounds.Encapsulate(maxWorld);
            bounds.Encapsulate(minMaxFront);
            bounds.Encapsulate(minMaxBack);

            return bounds;
        }
    }
}
