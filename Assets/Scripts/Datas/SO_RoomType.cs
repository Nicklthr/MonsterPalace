using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoomType", menuName = "MonsterPalace/Room Type")]
public class SO_RoomType : ScriptableObject
{
    public string roomName;

    [Header("Room size")]
    [Space(10)]
    public Vector3Int roomSize;

    public RoomType roomType;
    public ActivityType activityType;

    [Space(10)]
    public int maxUsers;

    [Space(10)]
    public int cost;
    public GameObject prefab;
    public bool isUnlocked = false;
}

[Serializable]
public class Room
{
    public SO_RoomType roomType;
    public Vector3Int positionInGrid;

    public Vector3Int roomSize;
    public string roomName;

    public ActivityType activityType;
    public RoomType type;
    public RoomPlacement[] roomPlacement;
    public SO_Food foodAssigned;

    public int maxUsers;
    public int currentUsers;

    public int cost;

    public GameObject roomObject;
    public List<TargetInRoom> targets;

    public Room( SO_RoomType roomType, Vector3Int positionInGrid )
    {
        this.roomType = roomType;
        this.positionInGrid = positionInGrid;
        this.roomSize = roomType.roomSize;
        this.roomName = roomType.roomName;
        this.type = roomType.roomType;
        this.maxUsers = roomType.maxUsers;
        
        if ( roomType.roomType == RoomType.ACTIVITY )
        {
            this.activityType = roomType.activityType;
        }

        this.targets = new List<TargetInRoom>();
        foreach ( Transform child in roomType.prefab.transform )
        {
            if ( child.CompareTag( "Target" ) )
            {
                this.targets.Add( new TargetInRoom( child.position, false ) );
            }
        }

        this.cost = roomType.cost;
        this.roomObject = roomType.prefab;
    }
}

[Serializable]
public class TargetInRoom
{
    [field: SerializeField]
    public Vector3 target { get; private set; }
    [field: SerializeField]
    public bool isOccupied { get; private set; }

    public TargetInRoom(Vector3 target, bool isOccupied)
    {
        this.target = target;
        this.isOccupied = isOccupied;
    }

    public void SetIsOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }
}