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
    }
}
