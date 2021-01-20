using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoom", menuName = "Level Generation/New Room", order = 131)]
public class RoomContainer : ScriptableObject
{
    #region Fields

    [SerializeField]
    private Room room;

    #endregion

    #region Properties

    public Room Room => room;

    #endregion
}
