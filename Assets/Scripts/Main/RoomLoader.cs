using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoguelikeVR
{
    public class RoomLoader : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Tunnel tunnelPrefab;

        #endregion

        #region Properties
    
        #endregion

        public void Load(RoomContainer roomContainer, System.Action<Room> onFinished = null)
        {
            var loadProgress = SceneManager.LoadSceneAsync(roomContainer.RoomScene, LoadSceneMode.Additive);
            StartCoroutine(WaitAndFinishLoading(roomContainer, loadProgress, onFinished));
        }

        private IEnumerator WaitAndFinishLoading(RoomContainer container, AsyncOperation operation, System.Action<Room> onFinished)
        {
            yield return null;
            yield return new WaitUntil(() => operation.isDone);
            LoadingFinished(container, onFinished);
        }

        private void LoadingFinished(RoomContainer container, System.Action<Room> onFinished)
        {
            var sceneLoaded = SceneManager.GetSceneByName(container.RoomScene);
            var roots = sceneLoaded.GetRootGameObjects();

            Room roomBehaviour = null;

            foreach (var r in roots)
            {
                roomBehaviour = r.GetComponent<Room>();

                if (roomBehaviour != null)
                {
                    break;
                }
            }

            if (roomBehaviour != null)
            {
                CreateTunnels(roomBehaviour);
            }

            onFinished?.Invoke(roomBehaviour);
        }

        private void CreateTunnels(Room room)
        {
            var tunnels = new Dictionary<Exit, Tunnel>();

            foreach (var e in room.Exits)
            {
                var tunnel = Instantiate(tunnelPrefab, e.transform.position, e.transform.rotation, e.transform);
                tunnels.Add(e, tunnel);
            }

            room.SetTunnels(tunnels);
        }
    }
}
