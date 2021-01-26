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
        private TunnelContainer tunnelContainer;

        [SerializeField]
        private int roomsCount;

        [SerializeField]
        private bool generatePreview;

        [SerializeField]
        private MeshRenderer connectionBoxPrefab;

        private List<RoomNode> roomStructure = new List<RoomNode>();
        private System.Action onFinished;
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
            var startNode = sourceNodes.Random();
            roomStructure.Add(startNode);
            sourceNodes.Remove(startNode);
            GenerateNodeStructure();
            ClearUnusedNodes();

            if (generatePreview)
            {
                CreatePreview();
            }
        }

        private void FinishedLoading()
        {
            onFinished?.Invoke();
            onFinished = null;
        }

        private void GenerateNodeStructure()
        {
            for (int i = 0; i < roomsCount - 1; i++)
            {
                if (sourceNodes.Count == 0)
                {
                    break;
                }

                var withOpenedExits = roomStructure.Where(r => r.OpenExits.Count > 0);

                if (withOpenedExits.Count() > 0)
                {
                    var outNode = withOpenedExits.ToArray().Random();
                    var inNode = sourceNodes.Random();

                    var exit = outNode.OpenExits.Random();
                    var enter = Random.Range(0, inNode.ExitCount);
                    SetupConnection(outNode, inNode, exit, enter);
                    ResolveExits();

                    roomStructure.Add(inNode);
                    sourceNodes.Remove(inNode);
                }
                else break;
            }

            FinishedLoading();
        }

        private void GenerateSourceNodes()
        {
            sourceNodes = new List<RoomNode>();
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
                sourceNodes.Add(node);
            }
        }

        private void SetupConnection(RoomNode outNode, RoomNode inNode, int exitIndex, int enterIndex)
        {
            var exit = outNode.Holder.Exits[exitIndex];
            var enter = inNode.Holder.Exits[enterIndex];

            Quaternion enterLookRotation = Quaternion.LookRotation(-exit.forward, Vector3.up);
            Quaternion inRoomLookRotation = inNode.transform.rotation * enterLookRotation;
            inNode.transform.rotation = inRoomLookRotation;

            Vector3 exitsDelta = exit.position - enter.position;
            inNode.transform.position += exitsDelta;

            var connection = new RoomConnection();
            connection.OtherRoomIndex = inNode.ThisRoomIndex;
            connection.ThisExitIndex = exitIndex;
            connection.OtherExitIndex = enterIndex;

            outNode.Connections.Add(connection);
            inNode.Connections.Add(connection);

            outNode.OpenExits.Remove(exitIndex);
            inNode.OpenExits.Remove(enterIndex);
        }

        private void ResolveExits()
        {
            var resolver = new ExitResolver(roomStructure);
            var possibleConnections = resolver.ResolveExits();

            foreach (var c in possibleConnections)
            {
                SetupConnection(roomStructure[c.ExitNode], roomStructure[c.EnterNode], c.ExitIndex, c.EnterIndex);
                CreateBounds(c);
            }
        }

        private void GeneratePlaceholder(RoomNode node)
        {
            var holderObject = new GameObject(node.Name);
            var placeholder = holderObject.AddComponent<RoomPlaceholder>();
            var room = roomsContainer.Variants[node.ThisRoomIndex];
            placeholder.Setup(node, room.Prefab.Bounds, room.Prefab.Exits);
            node.Holder = placeholder;
        }

        private void CreatePreview()
        {
            foreach (var r in roomStructure)
            {
                var prefab = roomsContainer.Variants[r.ThisRoomIndex].Prefab;
                Instantiate(prefab.View, r.transform.position, r.transform.rotation, r.Holder.transform);
            }
        }

        private void ClearUnusedNodes()
        {
            foreach (var n in sourceNodes)
            {
                Destroy(n.Holder.gameObject);
            }

            sourceNodes.Clear();
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
    }
}
