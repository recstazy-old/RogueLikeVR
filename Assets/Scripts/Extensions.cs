using GameOn.UnityHelpers;
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

        public static Bounds EncapsulateAllChildren(this GameObject parent)
        {
            var bounds = new Bounds();
            var renderers = parent.GetComponentsInChildren<MeshRenderer>();

            if (renderers.Length > 0)
            {
                bounds = renderers[0].bounds;
            }

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
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

        public static List<RoomConnection> RemoveInverts(this IEnumerable<RoomConnection> connections)
        {
            var result = new List<RoomConnection>();

            foreach (var c in connections)
            {
                bool canAdd = true;

                foreach (var r in result)
                {
                    if (c.IsInvertOf(r))
                    {
                        canAdd = false;
                        break;
                    }
                }

                if (canAdd)
                {
                    result.Add(c);
                }
            }

            return result;
        }

        public static List<RoomConnection> GetAllUniqConnections(this IEnumerable<RoomNode> nodes)
        {
            var connections = new List<RoomConnection>();

            if (nodes == null || nodes.Count() == 0)
            {
                return connections;
            }

            foreach (var n in nodes)
            {
                foreach (var c in n.Connections)
                {
                    connections.Add(c);
                }
            }

            connections = connections.RemoveInverts();
            return connections;
        }
    }
}
