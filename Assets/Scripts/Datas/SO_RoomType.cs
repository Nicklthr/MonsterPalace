using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewRoomType", menuName = "MonsterPalace/Room Type")]
public class SO_RoomType : ScriptableObject
{
    public string roomName;

    [Header("Room size")]
    [Space(10)]
    public Vector3Int roomSize;

    public RoomType roomType;
    public ActivityType activityType;
    public MonsterType monsterTypeOfRoom;

    [Space(10)]
    public int maxUsers;

    [Space(10)]
    public int cost;
    public int coinCost;
    public int quantityBuyable = 1;
    public GameObject prefab;
    public bool isUnlocked = false;
    public Sprite roomSprite;
}

[Serializable]
public class Room
{
    public SO_RoomType roomType;
    public Vector3Int positionInGrid;

    public string roomID;

    public Vector3Int roomSize;
    public string roomName;

    public ActivityType activityType;
    public RoomType type;
    public MonsterType monsterTypeOfRoom;
    public RoomPlacement[] roomPlacement = new RoomPlacement[0];
    public SO_Food foodAssigned;
    public string monsterID;

    public SO_Monster monsterDataCurrentCustomer;

    public int maxUsers;
    public int currentUsers;

    public int cost;

    public GameObject roomObject;
    public List<TargetInRoom> targets;

    public int level = 0;
    private int roomNumber;
    private GameObject objectInstance;

    public GameObject ObjectInstance => objectInstance;
    public int RoomNumber => roomNumber;

    public Room( SO_RoomType roomType, Vector3Int positionInGrid, string roomId, int level = 0 )
    {
        this.roomType = roomType;
        this.positionInGrid = positionInGrid;
        this.roomSize = roomType.roomSize;
        this.roomName = roomType.roomName;
        this.type = roomType.roomType;
        this.maxUsers = roomType.maxUsers;
        this.roomID = roomId;
        this.level = level;
        
        this.activityType = roomType.activityType;

        this.monsterTypeOfRoom = roomType.monsterTypeOfRoom;

        this.targets = new List<TargetInRoom>();

        this.cost = roomType.cost;
        this.roomObject = roomType.prefab;
    }

    public void SetRoomNumber(int roomNumber)
    {
        this.roomNumber = roomNumber;
    }

    public void SetObjectInstance(GameObject objectInstance)
    {
        this.objectInstance = objectInstance;
    }

    public void AddRoomPlacement( RoomPlacement newPlacement )
    {
        // Cr�er un nouveau tableau avec une taille augment�e de 1
        RoomPlacement[] newPlacements = new RoomPlacement[roomPlacement.Length + 1];

        // Copier les �l�ments existants
        for (int i = 0; i < roomPlacement.Length; i++)
        {
            newPlacements[i] = roomPlacement[i];
        }

        // Ajouter le nouvel �l�ment
        newPlacements[roomPlacement.Length] = newPlacement;

        // Assigner le nouveau tableau
        roomPlacement = newPlacements;
    }

    public void RemoveRoomPlacement( RoomPlacement placementToRemove )
    {
        // Trouver l'index de l'�l�ment � supprimer
        int index = Array.IndexOf( roomPlacement, placementToRemove );
        if (index < 0) return; // Si l'�l�ment n'est pas trouv�, ne rien faire

        // Cr�er un nouveau tableau avec une taille r�duite de 1
        RoomPlacement[] newPlacements = new RoomPlacement[roomPlacement.Length - 1];

        // Copier les �l�ments en omettant l'�l�ment � supprimer
        for (int i = 0, j = 0; i < roomPlacement.Length; i++)
        {
            if (i == index) continue;
            newPlacements[j++] = roomPlacement[i];
        }

        // Assigner le nouveau tableau
        roomPlacement = newPlacements;
    }

    public bool CheckInMonster(MonsterController monsterController)
    {
        if (monsterController == null)
        {
            Debug.LogError("Room: MonsterController is not assigned");
            return false;
        }

        if (!monsterController.canAssignRoom)
        {
            Debug.LogError("Room: MonsterController cannot assign room");
            return false;
        }

        if (currentUsers >= maxUsers)
        {
            Debug.LogError("Room: Room is full");
            return false;
        }

        // Assign monster to the room
        monsterID = monsterController.monsterID;
        monsterDataCurrentCustomer = monsterController.monsterDatas;
        currentUsers++;

        TargetInRoom targetInRoom = targets.FindLast(target => target.isOccupied == false);

        if (targetInRoom != null)
        {
            monsterController.roomPosition = targetInRoom.target;
            targetInRoom.SetIsOccupied(true);
        }
        else
        {
            Debug.LogError("Room: No target available");
            return false;
        }

        monsterController.roomAssigned = true;

        // Toggle room lights
        roomObject.GetComponentInChildren<RoomController>().ToggleLights();
        return true;
    }

    public bool CheckOutMonster(string monsterID)
    {
        if (this.monsterID != monsterID)
        {
            Debug.LogError("Room: This monster is not assigned to this room");
            return false;
        }

        // Reset room properties
        this.monsterID = null;
        this.currentUsers--;
        this.monsterDataCurrentCustomer = null;
        this.foodAssigned = null;

        TargetInRoom targetInRoom = targets.Find(target => target.isOccupied == true);
        if (targetInRoom != null)
        {
            targetInRoom.SetIsOccupied(false);
        }

        // Toggle room lights
        if (roomObject != null)
        {
            RoomController roomController = roomObject.GetComponentInChildren<RoomController>();
            roomController.ToggleLights();
        }

        return true;
    }
    public bool OccupiesPosition(Vector3Int checkPosition)
    {
        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                if (positionInGrid + new Vector3Int(x, y, 0) == checkPosition)
                {
                    return true;
                }
            }
        }
        return false;
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