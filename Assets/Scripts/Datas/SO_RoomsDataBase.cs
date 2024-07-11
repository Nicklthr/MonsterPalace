using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu( fileName = "RoomsDataBase", menuName = "MonsterPalace/RoomsDataBase" )]
public class SO_RoomsDataBase : ScriptableObject
{
    public List<RoomData> roomsData;
}

[Serializable]
public class RoomData
{
    [field: SerializeField]
    public string roomName { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int roomSize { get; private set; }
    [field: SerializeField]
    public GameObject prefab { get; private set; }

}
