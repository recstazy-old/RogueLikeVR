using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class ExitResolver : MonoBehaviour
    {
        public struct ConnectionData
        {
            public int ExitNode;
            public int EnterNode;
            public int ExitIndex;
            public int EnterIndex;
            public Bounds ConnectionBounds;
        }

        #region Fields

        private List<RoomNode> nodes;

        #endregion

        #region Properties
    
        #endregion

        public ExitResolver(List<RoomNode> nodes)
        {
            this.nodes = nodes;
        }

        public List<ConnectionData> ResolveExits()
        {
            List<ConnectionData> possibleConnections = new List<ConnectionData>();

            RoomNode exitNode;
            RoomNode enterNode;
            Transform exit;
            Transform enter;
            Collider[] hits = new Collider[3];

            for (int i = 0; i < nodes.Count; i++)
            {
                exitNode = nodes[i];

                for (int j = 0; j < nodes.Count; j++)
                {
                    if (i == j) continue;

                    enterNode = nodes[j];

                    foreach (var exitI in nodes[i].OpenExits)
                    {
                        exit = exitNode.Holder.Exits[exitI].transform;

                        foreach (var enterI in nodes[j].OpenExits)
                        {
                            enter = enterNode.Holder.Exits[enterI].transform;

                            float forwardDot = Vector3.Dot(exit.forward, enter.forward);
                            float forwardToDirectionDot = Vector3.Dot(exit.forward, (enter.position - exit.position).normalized);

                            if (forwardDot < -0.5f && forwardToDirectionDot > 0.5f)
                            {
                                var exitLeft = exit.transform.position - exit.transform.right * exit.localScale.x * 0.5f;
                                var exitRight = exitLeft + exit.right * exit.localScale.x;
                                var enterLeft = enter.transform.position - enter.transform.right * enter.localScale.x * 0.5f;
                                var enterRight = enterLeft + enter.right * enter.localScale.x;
                                var height = Mathf.Max(enter.localScale.y, exit.localScale.y);
                                var top = enterLeft + Vector3.up * height;

                                var box = new Bounds(exitLeft, Vector3.zero);
                                box.Encapsulate(exitRight);
                                box.Encapsulate(enterLeft);
                                box.Encapsulate(enterRight);
                                box.Encapsulate(top);

                                bool overlappedConnections = false;

                                foreach (var c in possibleConnections)
                                {
                                    overlappedConnections = box.Intersects(c.ConnectionBounds);

                                    if (overlappedConnections)
                                    {
                                        break;
                                    }
                                }

                                bool overlappedAnything;
                                bool overlappedRooms = false;

                                if (!overlappedConnections)
                                {
                                    Physics.OverlapBox(box.center, box.extents, Quaternion.identity, LayerMask.GetMask("Bounds"), QueryTriggerInteraction.Collide);
                                    overlappedRooms = hits[2] != null;
                                    hits[2] = null;
                                }

                                overlappedAnything = overlappedConnections || overlappedRooms;

                                if (!overlappedAnything)
                                {
                                    var data = new ConnectionData();
                                    data.ExitNode = i;
                                    data.EnterNode = j;
                                    data.ExitIndex = exitI;
                                    data.EnterIndex = enterI;
                                    data.ConnectionBounds = box;

                                    possibleConnections.Add(data);
                                }
                            }
                        }
                    }
                }
            }

            return possibleConnections;
        }
    }
}
