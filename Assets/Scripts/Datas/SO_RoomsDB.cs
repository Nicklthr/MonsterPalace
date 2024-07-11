using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomsDB", menuName = "MonsterPalace/RoomsDB")]
public class SO_RoomsDB : ScriptableObject
{
    public List<RoomDB> rooms;
}

[Serializable]
public class RoomDB
{
    [field: SerializeField]
    public List<SO_RoomType> rooms { get; private set; }
    [field: SerializeField]
    public RoomType RoomType { get; private set; }
}
