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
    public int cost;
    public GameObject prefab;
    public bool isUnlocked = false;
}

public class Room
{
    public SO_RoomType roomType;
    public Vector3Int positionInGrid;

    public Vector3Int roomSize;
    public string roomName;

    public ActivityType activityType;
    public RoomType type;
    public RoomPlacement roomPlacement;

    public int cost;

    public GameObject roomObject;

    public Room( SO_RoomType roomType, Vector3Int positionInGrid )
    {
        this.roomType = roomType;
        this.positionInGrid = positionInGrid;
        this.roomSize = roomType.roomSize;
        this.roomName = roomType.roomName;
        this.type = roomType.roomType;
        
        if ( roomType.roomType == RoomType.ACTIVITY )
        {
            this.activityType = roomType.activityType;
        }

        this.cost = roomType.cost;
        this.roomObject = roomType.prefab;
    }

}