using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace RoguelikeVR.LevelGeneration
{
    public class NavMeshBaker
    {
        #region Fields
    
        #endregion

        #region Properties
    
        #endregion

        public void PrepareAndBake(List<RoomNode> rooms)
        {
            var surfaceHolder = new GameObject("Navigation");
            var navSurface = surfaceHolder.AddComponent<NavMeshSurface>();
            navSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
            navSurface.BuildNavMesh();
        }
    }
}
