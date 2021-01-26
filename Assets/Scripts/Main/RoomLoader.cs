using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoguelikeVR
{
    public class RoomLoader : MonoBehaviour
    {
        #region Fields

        //private static RoomLoader instance;

        #endregion

        #region Properties

        //private static RoomLoader Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            CreateInstance();
        //        }

        //        return instance;
        //    }
        //}

        #endregion

        //public static void LoadRoom(RoomContainer container, System.Action<Room> onFinished)
        //{
        //    Instance.Load(container, onFinished);
        //}

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

            onFinished?.Invoke(roomBehaviour);
        }

        //private static void CreateInstance()
        //{
        //    var loaderObject = new GameObject("RoomLoader");
        //    instance = loaderObject.AddComponent<RoomLoader>();
        //}
    }
}
