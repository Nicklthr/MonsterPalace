using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HotelController : MonoBehaviour
{
    public static HotelController Instance { get; private set; }

    [SerializeField] private SO_Hotel _hotel;
    [SerializeField] private SO_RoomType _dataReception;
    [SerializeField] private SO_RoomType _stairRoom;
    [SerializeField] private Grid _grid;
    [SerializeField] public UnityEvent OnHotelCreated = new UnityEvent();
    [SerializeField] private int _maxStage = 0;
    [SerializeField] private int _minStage = 0;

    public int MaxStage => _maxStage;
    public int MinStage => _minStage;

    public event Action OnHotelCreate;
    public event Action<Room> OnRoomCreated;
    public event Action<Room> OnRoomDestroyed;
    public event Action OnStairPlaced;
    public UnityEvent OnStairBuild = new UnityEvent();

    private MoneyManager _moneyManager;

    private Dictionary<int, HashSet<Vector2Int>> _occupiedPositions;

    private const int LEVEL_HEIGHT = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        _occupiedPositions = new Dictionary<int, HashSet<Vector2Int>>();
        _moneyManager = FindObjectOfType<MoneyManager>();
    }

    private void Start()
    {
        InitializeHotel();
    }

    public int GetLevelHeight()
    {
        return LEVEL_HEIGHT;
    }

    public void InitializeHotel()
    {
        _hotel.rooms.Clear();
        _occupiedPositions.Clear();
        AddBaseRoom();
        OnHotelCreate?.Invoke();
        OnHotelCreated.Invoke();
    }

    private void AddBaseRoom()
    {
        AddNewRoom(_dataReception, new Vector3Int(0, 0, 0));
        GameObject reception = GameObject.FindGameObjectWithTag("ReceptionPosition");
        if (reception != null)
        {
            reception.SetActive(true);
        }
    }

    public bool CanPlaceRoomAt(Vector3Int position, Vector2Int size)
    {
        int level = Mathf.RoundToInt(position.y / (float)LEVEL_HEIGHT);

        // Ensure the Y position matches the level
        if (position.y != level * LEVEL_HEIGHT)
        {
            return false;
        }

        if (!_occupiedPositions.ContainsKey(level))
        {
            return level == 0; // Only allow placement on ground level if it's empty
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int checkPos = new Vector2Int(position.x + x, position.z + z);
                if (_occupiedPositions[level].Contains(checkPos))
                {
                    return false;
                }
            }
        }

        // Check for adjacent rooms on the same level
        bool hasAdjacentRoom = false;
        foreach (Vector2Int adjacentPos in GetAdjacentPositions(position, size))
        {
            if (_occupiedPositions[level].Contains(adjacentPos))
            {
                hasAdjacentRoom = true;
                break;
            }
        }

        return hasAdjacentRoom;
    }

    private IEnumerable<Vector2Int> GetAdjacentPositions(Vector3Int position, Vector2Int size)
    {
        for (int x = -1; x <= size.x; x++)
        {
            yield return new Vector2Int(position.x + x, position.z - 1);
            yield return new Vector2Int(position.x + x, position.z + size.y);
        }
        for (int z = 0; z < size.y; z++)
        {
            yield return new Vector2Int(position.x - 1, position.z + z);
            yield return new Vector2Int(position.x + size.x, position.z + z);
        }
    }

    public void AddNewRoom(SO_RoomType roomType, Vector3Int position, RoomPlacement locationType = RoomPlacement.OVERGROUND, RoomPlacement lightingCondition = RoomPlacement.LIGHT)
    {
        int level = Mathf.RoundToInt(position.y / (float)LEVEL_HEIGHT);
        position.y = level * LEVEL_HEIGHT; // Ensure the Y position is correct for the level

        string roomID = System.Guid.NewGuid().ToString();
        Room newRoom = new Room(roomType, position, roomID, _hotel.rooms.Count);
        _hotel.AddRoom(newRoom);
        newRoom.AddRoomPlacement(locationType);
        newRoom.AddRoomPlacement(lightingCondition);

        if (!_occupiedPositions.ContainsKey(level))
        {
            _occupiedPositions[level] = new HashSet<Vector2Int>();
        }

        for (int x = 0; x < roomType.roomSize.x; x++)
        {
            for (int z = 0; z < roomType.roomSize.y; z++)
            {
                _occupiedPositions[level].Add(new Vector2Int(position.x + x, position.z + z));
            }
        }

        InstantiateRoom(newRoom);
        OnRoomCreated?.Invoke(newRoom);
    }

    private void InstantiateRoom(Room room)
    {
        GameObject prefab = room.roomType.prefab;
        GameObject roomInstance = Instantiate(prefab, _grid.CellToWorld(room.positionInGrid), Quaternion.identity, transform);
        
        room.SetObjectInstance(roomInstance);

        roomInstance.GetComponent<RoomController>().SetRoom(room);
        roomInstance.GetComponent<RoomController>().ToggleLights();
        foreach (Transform child in roomInstance.transform)
        {
            if (child.CompareTag("Target"))
            {
                room.targets.Add(new TargetInRoom(child.position, false));
            }
        }
        roomInstance.name = room.roomID;
    }

    public void RemoveRoom(string roomID)
    {
        Room roomToRemove = _hotel.rooms.Find(r => r.roomID == roomID);
        if (roomToRemove != null)
        {
            int level = Mathf.RoundToInt(roomToRemove.positionInGrid.y / (float)LEVEL_HEIGHT);
            for (int x = 0; x < roomToRemove.roomType.roomSize.x; x++)
            {
                for (int z = 0; z < roomToRemove.roomType.roomSize.y; z++)
                {
                    _occupiedPositions[level].Remove(new Vector2Int(roomToRemove.positionInGrid.x + x, roomToRemove.positionInGrid.z + z));
                }
            }

            _hotel.rooms.Remove(roomToRemove);
            GameObject roomObject = GameObject.Find(roomID);
            if (roomObject != null)
            {
                Destroy(roomObject);
                OnRoomDestroyed?.Invoke(roomToRemove);
            }
        }
    }

    public void AddUnderStair()
    {
        if (!CheckIfEnoughMoney(_stairRoom.cost)) return;

        int newLevel = _minStage - 1;
        Vector3Int position = new Vector3Int(0, newLevel * LEVEL_HEIGHT, 0);

        AddNewRoom( _stairRoom, position, RoomPlacement.UNDERGROUND, RoomPlacement.DARK );

        Debug.Log("J'ai passé la fonction AddNewRoom dans AddUnderStair");

        Room newStairRoom = _hotel.rooms.Last();
        GameObject newStairObject = newStairRoom.ObjectInstance;

        Room aboveRoom = _hotel.rooms.FindLast(x => x.level == _minStage && x.roomType.roomType == RoomType.BASE);
        if (aboveRoom != null)
        {
            GameObject aboveRoomObject = GameObject.Find(aboveRoom.roomID);
            aboveRoomObject.GetComponent<StairCaseController>().DesactivateGround();

            if (_minStage == 0)
            {
                aboveRoomObject.GetComponent<StairCaseController>().DesactivateStairWall();
                newStairObject.GetComponent<StairCaseController>().ActivateStarMiniWall();

                if (CheckIfBaseRoomUpper(0))
                {
                    newStairObject.GetComponent<StairCaseController>().DesactivateStarMiniWall();
                }
            }
            else
            {
                newStairObject.GetComponent<StairCaseController>().DesactivateStarMiniWall();
            }
        }

        newStairObject.GetComponent<StairCaseController>().ActivateStair();
        newStairObject.GetComponent<StairCaseController>().ActivateGround();

        _moneyManager.PayRoom(_stairRoom.cost);
        AddBasement();

        OnStairBuild.Invoke();
        OnStairPlaced?.Invoke();
    }

    public void AddUpperStair()
    {
        if (!CheckIfEnoughMoney(_stairRoom.cost))
        {
            return;
        }

        int newLevel = _maxStage + 1;
        Vector3Int position = new Vector3Int(0, newLevel * LEVEL_HEIGHT, 0);

        AddNewRoom(_stairRoom, position, RoomPlacement.OVERGROUND, RoomPlacement.LIGHT);

        Room newStairRoom = _hotel.rooms.Last();
        GameObject newStairObject = GameObject.Find(newStairRoom.roomID);

        Room belowRoom = _hotel.rooms.FindLast(x => x.level == _maxStage && x.roomType.roomType == RoomType.BASE);
        if (belowRoom != null)
        {
            GameObject belowRoomObject = GameObject.Find(belowRoom.roomID);
            belowRoomObject.GetComponent<StairCaseController>().ActivateStair();

            if (newLevel == 1)
            {
                Room groundFloorRoom = _hotel.rooms.Find(x => x.level == 0 && x.roomType.roomType == RoomType.BASE);
                if (groundFloorRoom != null)
                {
                    GameObject.Find(groundFloorRoom.roomID).GetComponent<StairCaseController>().DesactivateStarMiniWall();
                }

                if (CheckIfBaseRoomBelow(0))
                {
                    GetBaseRoomLevel(-1).GetComponent<StairCaseController>().DesactivateStarMiniWall();
                }

                belowRoomObject.GetComponent<StairCaseController>().DesactivateStairWall();
                belowRoomObject.GetComponent<StairCaseController>().ActivateStarMiniWall();
            }
            else if (newLevel > 1)
            {
                belowRoomObject.GetComponent<StairCaseController>().ActivateStarMiniWall();

                Room twoLevelsBelow = _hotel.rooms.Find(x => x.level == newLevel - 2 && x.roomType.roomType == RoomType.BASE);
                if (twoLevelsBelow != null)
                {
                    GameObject.Find(twoLevelsBelow.roomID).GetComponent<StairCaseController>().DesactivateStarMiniWall();
                }
            }
        }

        newStairObject.GetComponent<StairCaseController>().DesactivateStarMiniWall();
        newStairObject.GetComponent<StairCaseController>().DesactivateGround();

        _moneyManager.PayRoom(_stairRoom.cost);
        AddStage();

        OnStairBuild.Invoke();
        OnStairPlaced?.Invoke();
    }

    public void AddStage()
    {
        _maxStage++;
    }

    public void AddBasement()
    {
        _minStage--;
    }

    private bool CheckIfEnoughMoney(float cost)
    {
        return _moneyManager.playerMoney >= cost;
    }

    private bool CheckIfBaseRoomUpper(int level)
    {
        return _hotel.rooms.Any(x => x.level > level && x.roomType.roomType == RoomType.BASE);
    }

    private bool CheckIfBaseRoomBelow(int level)
    {
        return _hotel.rooms.Any(x => x.level < level && x.roomType.roomType == RoomType.BASE);
    }

    private GameObject GetBaseRoomLevel(int level)
    {
        Room room = _hotel.rooms.Find(x => x.level == level && x.roomType.roomType == RoomType.BASE);
        return room != null ? GameObject.Find(room.roomID) : null;
    }

    public List<Room> GetRooms() => _hotel.rooms;

    public Room GetRoomByMonsterID(string monsterID) => _hotel.rooms.Find(r => r.monsterID == monsterID);

    public int GetLevelFromPosition(Vector3Int position)
    {
        return Mathf.RoundToInt(position.y / (float)LEVEL_HEIGHT);
    }

    public Vector3Int AdjustPositionToLevel(Vector3Int position)
    {
        int level = GetLevelFromPosition(position);
        return new Vector3Int(position.x, level * LEVEL_HEIGHT, position.z);
    }
}