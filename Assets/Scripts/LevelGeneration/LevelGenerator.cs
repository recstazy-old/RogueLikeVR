using GameOn.UnityHelpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
        [Range(0.01f, 1f)]
        private float doorsFraction;

        [SerializeField]
        private Door doorPrefab;

        [SerializeField]
        private Transform doorsParent;

        [SerializeField]
        [Range(0.01f, 1f)]
        private float enemyFraction;

        [SerializeField]
        private GameObject enemyPrefab;

        [SerializeField]
        private Transform enemiesParent;

        private List<RoomNode> roomStructure = new List<RoomNode>();
        private System.Action onFinished;
        private List<RoomNode> availableNodes;
        private List<RoomNode> sourceNodes;
        private Dictionary<int, RoomNode> configToStructureIndices = new Dictionary<int, RoomNode>();

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
            configToStructureIndices.Add(startNode.ThisRoomIndex, startNode);
            availableNodes.Remove(startNode);
            GenerateNodeStructure();
        }

        private void FinishedLoading()
        {
            ClearUnusedNodes();
            CloseUnusedExits();
            var baker = new NavMeshBaker();
            baker.PrepareAndBake(roomStructure);

            CreateDynamicDoors();
            CreateEnemies();

            ReplaceHoldersWithScenes();

            onFinished?.Invoke();
            onFinished = null;
        }

        private void GenerateNodeStructure()
        {
            int attempt = 0;

            while (roomStructure.Count < roomsCount && attempt < 100)
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
                        continue;
                    }

                    roomStructure.Add(inNode);
                    configToStructureIndices.Add(inNode.ThisRoomIndex, inNode);
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

            Quaternion enterTargetRotation = Quaternion.LookRotation(-exit.transform.forward, Vector3.up);
            inNode.transform.rotation *= enterTargetRotation * Quaternion.Inverse(enter.transform.rotation);

            Vector3 exitsDelta = exit.transform.position - enter.transform.position;
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

        private void CreateDynamicDoors()
        {
            var startRoom = roomStructure.FirstOrDefault();

            if (startRoom != null)
            {
                var connectionsOfFirstRoom = startRoom.Connections;

                foreach (var c in connectionsOfFirstRoom)
                {
                    CloseConnectionWithDynamicDoor(c);
                }

                var connections = roomStructure.GetAllUniqConnections();
                connections = connections.Where(c =>
                {
                    foreach (var firstC in connectionsOfFirstRoom)
                    {
                        if (firstC.Equal(c))
                        {
                            return false;
                        }

                        if (firstC.IsInvertOf(c))
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .ToList();

                int period = Mathf.FloorToInt(1f / doorsFraction);

                for (int i = 0; i < connections.Count; i += period)
                {
                    CloseConnectionWithDynamicDoor(connections[i]);
                }
            }
        }

        private void CloseConnectionWithDynamicDoor(RoomConnection connection)
        {
            var exit = configToStructureIndices[connection.ThisRoomIndex].Holder.Exits[connection.ThisExitIndex];
            Instantiate(doorPrefab, exit.transform.position, exit.transform.rotation, doorsParent);
        }

        private void CreateEnemies()
        {
            var period = Mathf.FloorToInt(1f / enemyFraction);

            for (int i = 1; i < roomStructure.Count; i += period)
            {
                Instantiate(enemyPrefab, roomStructure[i].transform.position, roomStructure[i].transform.rotation, enemiesParent);
            }
        }

        private void CloseUnusedExits()
        {
            foreach (var node in roomStructure)
            {
                foreach (var index in node.OpenExits)
                {
                    var exit = node.Holder.Exits[index];
                    exit.Close();
                }
            }
        }

        private void GeneratePlaceholder(RoomNode node)
        {
            var holderObject = new GameObject(node.Name);
            var placeholder = holderObject.AddComponent<RoomPlaceholder>();
            var room = roomsContainer.Variants[node.ThisRoomIndex];
            placeholder.Setup(node, room.Prefab);
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

        private void ReplaceHoldersWithScenes()
        {
            List<RoomNode> rooms = new List<RoomNode>();

            foreach (var r in roomStructure)
            {
                var roomVariant = roomsContainer.Variants[r.ThisRoomIndex];

                if (r.Name == "RoomCube4")
                {
                    rooms.Add(r);
                    SceneManager.LoadScene(roomVariant.RoomScene, LoadSceneMode.Additive);
                    
                }
            }

            this.WaitFramesAndRun(1, () => 
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    var handle = SceneManager.GetSceneAt(i + 1);
                    var gameObjects = new List<GameObject>();
                    handle.GetRootGameObjects(gameObjects);

                    foreach (var g in gameObjects)
                    {
                        g.transform.position = rooms[i].transform.position;
                        g.transform.rotation = rooms[i].transform.rotation;
                    }

                    rooms[i].Holder.gameObject.SetActive(false);
                }
            });
        }

        #region Gizmos

        // Gizmos
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
                    Vector3 exitPosition = r.Holder.Exits[c.ThisExitIndex].transform.position;
                    Vector3 enterPosition = sourceNodes[c.OtherRoomIndex].Holder.Exits[c.OtherExitIndex].transform.position;

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(roomPosition + moveUp, exitPosition + moveUp);
                    Gizmos.DrawLine(enterPosition + moveUp, otherRoomPosition + moveUp);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(exitPosition + moveUp, enterPosition + moveUp);
                    Gizmos.DrawSphere(exitPosition + moveUp, 0.03f);
                    Gizmos.DrawLine(exitPosition + moveUp, exitPosition + moveUp + r.Holder.Exits[c.ThisExitIndex].transform.forward * 0.3f);

                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(enterPosition + moveUp, 0.03f);
                    Gizmos.DrawLine(enterPosition + moveUp, enterPosition + moveUp + sourceNodes[c.OtherRoomIndex].Holder.Exits[c.OtherExitIndex].transform.forward * 0.3f);
                    drawnConnections.Add(c);
                }

                Gizmos.color = Color.magenta;
                var bounds = r.Holder.Bounds.GetWorldBoundsInXZPlane();
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }

        #endregion
    }
}
