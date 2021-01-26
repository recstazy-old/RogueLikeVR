using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR
{
    [CreateAssetMenu(fileName = "NewTunnelContainer", menuName = "Level Generation/New Tunnel Container", order = 131)]
    public class TunnelContainer : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private Tunnel[] variants;

        #endregion

        #region Properties

        public Tunnel[] Variants => variants;

        #endregion
    }
}
