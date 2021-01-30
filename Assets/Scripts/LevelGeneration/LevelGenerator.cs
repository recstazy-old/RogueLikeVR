using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoguelikeVR
{
    public class LevelGenerator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameConfig gameConfig;

        [SerializeField]
        private RoomVariantContainer roomsContainer;

        [SerializeField]
        private int roomsCount;

        [SerializeField]
        private bool generatePreview;

        [SerializeField]
        private MeshRenderer connectionBoxPrefab;

        [SerializeField]
        private bool debugPerStep;

        private List<RoomNode> roomStructure = new List<RoomNode>();
        private System.Action onFinished;
        private List<RoomNode> availableNodes;
        private List<RoomNode> sourceNodes;

        #endregion

        #region Properties

        public List<Room> Rooms { get; private set; } = new List<Room>();

        #endregion

        public void Generate(System.Action onFinished)
        {
            this.onFinished = onFinished;
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            GenerateSourceNodes();
            var startNode = availableNodes.Random();
            roomStructure.Add(startNode);
            availableNodes.Remove(startNode);

            if (!debugPerStep)
            {
                GenerateNodeStructure();
                ClearUnusedNodes();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(GenerateRoutine());
            }
        }

        private void FinishedLoading()
        {
            onFinished?.Invoke();
            onFinished = null;
        }

        private void GenerateNodeStructure()
        {
            int attempt = 0;

            while(roomStructure.Count < roomsCount && attempt < 100)
            {
                if (availableNodes.Count == 0)
                {
                    break;
                }

                var withOpenedExits = roomStructure.Where(r => r.OpenExits.Count > 0);

                if (withOpenedExits.Count() > 0)
                {
                    var outNode = withOpenedExits.ToArray().Random();
                    var inNode = availableNodes.Random();

                    var exit = outNode.OpenExits.Random();
                    var enter = Random.Range(0, inNode.ExitCount);
                    bool connected = TrySetupConnection(outNode, inNode, exit, enter);

                    if (!connected)
                    {
                        outNode.BlockedExits.Add(exit);
                        outNode.OpenExits.Remove(exit);
                        continue;
                    }

                    //ResolveExits();

                    roomStructure.Add(inNode);
                    availableNodes.Remove(inNode);
                }
                else break;

                attempt++;
            }

            if (attempt >= 100)
            {
                Debug.LogError("Reached 100 attempts");
            }

            FinishedLoading();
        }

        private IEnumerator GenerateRoutine()
        {
            int attempt = 0;

            while (roomStructure.Count < roomsCount && attempt < 100)
            {
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                
                if (availableNodes.Count == 0)
                {
                    break;
                }

                var withOpenedExits = roomStructure.Where(r => r.OpenExits.Count > 0);

                if (withOpenedExits.Count() > 0)
                {
                    var outNode = withOpenedExits.ToArray().Random();
                    var inNode = availableNodes.Random();

                    var exit = outNode.OpenExits.Random();
                    var enter = Random.Range(0, inNode.ExitCount);
                    bool connected = TrySetupConnection(outNode, inNode, exit, enter);

                    if (!connected)
                    {
                        outNode.BlockedExits.Add(exit);
                        outNode.OpenExits.Remove(exit);
                        continue;
                    }

                    //ResolveExits();

                    roomStructure.Add(inNode);
                    availableNodes.Remove(inNode);
                }
                else break;

                attempt++;
            }

            if (attempt >= 100)
            {
                Debug.LogError("Reached 100 attempts");
            }

            ClearUnusedNodes();
            FinishedLoading();
        }

        private void GenerateSourceNodes()
        {
            sourceNodes = new List<RoomNode>();
            availableNodes = new List<RoomNode>();
            var holdersParent = new GameObject("RoomsPlaceholders");

            for (int i = 0; i < roomsContainer.Variants.Length; i++)
            {
                var node = new RoomNode();
                node.ThisRoomIndex = i;
                node.ExitCount = roomsContainer.Variants[i].Prefab.Exits.Length;
                node.FillOpen();
                node.Name = roomsContainer.Variants[i].name;
                GeneratePlaceholder(node);
                node.Holder.transform.SetParent(holdersParent.transform);
                availableNodes.Add(node);
                sourceNodes.Add(node);
            }
        }

        private bool TrySetupConnection(RoomNode outNode, RoomNode inNode, int exitIndex, int enterIndex)
        {
            var exit = outNode.Holder.Exits[exitIndex];
            var enter = inNode.Holder.Exits[enterIndex];

            Quaternion enterTargetRotation = Quaternion.LookRotation(-exit.forward, Vector3.up);
            inNode.transform.rotation *= enterTargetRotation * Quaternion.Inverse(enter.rotation);

            Vector3 exitsDelta = exit.position - enter.position;
            inNode.transform.position += exitsDelta;

            bool overlaps = OverlapsStructure(inNode, outNode);

            if (overlaps)
            {
                inNode.transform.rotation = Quaternion.identity;
                inNode.transform.position = Vector3.zero;
            }
            else
            {
                var connection = new RoomConnection();
                connection.ThisRoomIndex = outNode.ThisRoomIndex;
                connection.OtherRoomIndex = inNode.ThisRoomIndex;
                connection.ThisExitIndex = exitIndex;
                connection.OtherExitIndex = enterIndex;
                outNode.Connections.Add(connection);

                connection.Invert();
                inNode.Connections.Add(connection);

                outNode.OpenExits.Remove(exitIndex);
                inNode.OpenExits.Remove(enterIndex);
            }

            return !overlaps;
        }

        private bool OverlapsStructure(RoomNode inNode, params RoomNode[] ignore)
        {
            Bounds nodeBounds = inNode.Holder.Bounds.GetWorldBoundsInXZPlane();
            bool overlapped = false;
            var roomStructure = this.roomStructure.Except(ignore);

            foreach (var r in roomStructure)
            {
                var rBounds = r.Holder.Bounds.GetWorldBoundsInXZPlane();
                overlapped = nodeBounds.Intersects(rBounds);

                if (overlapped)
                {
                    break;
                }
            }

            return overlapped;
        }

        private void ResolveExits()
        {
            var resolver = new ExitResolver(roomStructure);
            var possibleConnections = resolver.ResolveExits();

            foreach (var c in possibleConnections)
            {
                TrySetupConnection(roomStructure[c.ExitNode], roomStructure[c.EnterNode], c.ExitIndex, c.EnterIndex);
            }
        }

        private void GeneratePlaceholder(RoomNode node)
        {
            var holderObject = new GameObject(node.Name);
            var placeholder = holderObject.AddComponent<RoomPlaceholder>();
            var room = roomsContainer.Variants[node.ThisRoomIndex];

            if (generatePreview)
            {
                placeholder.Setup(node, room.Prefab);
            }
            else
            {
                placeholder.SetupWithoutView(node, room.Prefab);
            }
           
            node.Holder = placeholder;
        }

        private void ClearUnusedNodes()
        {
            foreach (var n in availableNodes)
            {
                Destroy(n.Holder.gameObject);
            }

            availableNodes.Clear();
        }

        private void CreateBounds(ExitResolver.ConnectionData data)
        {
            var exitNode = roomStructure[data.ExitNode];

            var boundsObj = new GameObject("Connection");
            var collider = boundsObj.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.gameObject.layer = LayerMask.NameToLayer("Bounds");
            collider.transform.position = data.ConnectionBounds.center;
            collider.size = data.ConnectionBounds.size;

            var connectionView = Instantiate(connectionBoxPrefab, boundsObj.transform);
            connectionView.transform.localPosition = Vector3.zero;
            connectionView.transform.localRotation = Quaternion.identity;
            connectionView.transform.localScale = collider.size;
            connectionView.material.color = new Color(1f, 0f, 1f, 0.5f);

            boundsObj.transform.SetParent(exitNode.Holder.Exits[data.ExitIndex]);
        }

        private List<RoomConnection> drawnConnections = new List<RoomConnection>();
        private void OnDrawGizmos()
        {
            float height = 0.5f;
            drawnConnections.Clear();

            foreach (var r in roomStructure)
            {
                foreach (var c in r.Connections)
                {
                    bool isDrawnAlready = drawnConnections.Where(con => con.IsInvertOf(c)).Count() > 0;
                    if (isDrawnAlready) 
                        continue;

                    height += 0.05f;
                    Vector3 moveUp = height * Vector3.up;

                    Vector3 roomPosition = r.transform.position;
                    Vector3 otherRoomPosition = sourceNodes[c.OtherRoomIndex].transform.position;
                    Vector3 exitPosition = r.Holder.Exits[c.ThisExitIndex].position;
                    Vector3 enterPosition = sourceNodes[c.OtherRoomIndex].Holder.Exits[c.OtherExitIndex].position;

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(roomPosition + moveUp, exitPosition + moveUp);
                    Gizmos.DrawLine(enterPosition + moveUp, otherRoomPosition + moveUp);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(exitPosition + moveUp, enterPosition + moveUp);
                    Gizmos.DrawSphere(exitPosition + moveUp, 0.03f);
                    Gizmos.DrawLine(exitPosition + moveUp, exitPosition + moveUp + r.Holder.Exits[c.ThisExitIndex].forward * 0.3f);

                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(enterPosition + moveUp, 0.03f);
                    Gizmos.DrawLine(enterPosition + moveUp, enterPosition + moveUp + sourceNodes[c.OtherRoomIndex].Holder.Exits[c.OtherExitIndex].forward * 0.3f);
                    drawnConnections.Add(c);
                }

                Gizmos.color = Color.magenta;
                var bounds = r.Holder.Bounds.GetWorldBoundsInXZPlane();
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }
}
