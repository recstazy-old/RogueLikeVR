using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    public class MakeStaticOnBake : MonoBehaviour
    {
        #region Fields
    
        #endregion

        #region Properties
    
        #endregion

        public void SetIsStatic(bool isStatic)
        {
            var objects = GetComponentsInChildren<Transform>();

            foreach (var o in objects)
            {
                o.gameObject.isStatic = isStatic;
            }
        }
    }
}
