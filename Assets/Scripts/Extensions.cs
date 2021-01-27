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

        public static Bounds ConvertToWorldBounds(this BoxCollider box)
        {
            Vector3 minWorld = box.transform.TransformPoint(box.center - box.size * 0.5f);
            Vector3 maxWorld = box.transform.TransformPoint(box.center + box.size * 0.5f);
            var bounds = new Bounds(minWorld, Vector3.zero);
            bounds.Encapsulate(maxWorld);
            return bounds;
        }
    }
}
