using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEditor;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _mouseIndicator;
    [SerializeField]
    private InputManager _inputManager;
    [SerializeField]
    private Grid _grid;
    [SerializeField]
    private SO_Hotel _hotel;
    [SerializeField]
    private SO_RoomType _dataBase;

    private SO_RoomType _selectedRoom;

    [SerializeField]
    private GameObject gridVisualization;

    private GameObject _roomIndicator;
    private PreviewRoom _previewRoom;

    public UnityEvent OnRoomPlaced;

    private void Start()
    {
        StopPlacement();
        _mouseIndicator.SetActive(false);
    }

    public void StartPlacement(SO_RoomType room)
    {
        StopPlacement();
        _selectedRoom = room;
        gridVisualization.SetActive(true);

        _roomIndicator = Instantiate(_selectedRoom.prefab, _mouseIndicator.transform.position, Quaternion.identity);
        _previewRoom = _roomIndicator.GetComponentInChildren<PreviewRoom>();


        _inputManager.OnClicked += PlaceRoom;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceRoom()
    {
        if (_inputManager.IsPointerOverUI())
            return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        Bounds localBounds = _grid.GetBoundsLocal(gridPosition, _selectedRoom.roomSize);
        if (IsPositionFree(localBounds))
        {
            if (IsRoomNearby(localBounds, gridPosition.y, _selectedRoom.roomSize.y, out string dir))
            {
                GameObject newRoom = Instantiate(_selectedRoom.prefab);
                newRoom.transform.position = _grid.CellToWorld(gridPosition);
                newRoom.name = newRoom.GetInstanceID().ToString();

                Room room = new Room(_selectedRoom, gridPosition, newRoom.GetInstanceID().ToString());

                room.AddRoomPlacement(gridPosition.y < 0 ? RoomPlacement.UNDERGROUND : RoomPlacement.OVERGROUND);

                if (dir == "right" || dir == "left" && gridPosition.y > 0)
                {
                    room.AddRoomPlacement( RoomPlacement.LIGHT );
                } else if (dir == "right" || dir == "left" && gridPosition.y < 0 )
                {
                    room.AddRoomPlacement(RoomPlacement.DARK);
                }else if ( dir == "both" && gridPosition.y > 0 )
                {
                    room.AddRoomPlacement(RoomPlacement.DARK);
                }else if (dir == "both" && gridPosition.y < 0)
                {
                    room.AddRoomPlacement(RoomPlacement.DARK);
                }

                _hotel.AddRoom(room);
                OnRoomPlaced.Invoke();

            } else
            {
                Debug.Log("Room is too far from other rooms");
            }
        }
        else
        {
            Debug.Log("Position is not free");
        }
    }

    private void StopPlacement()
    {
        _selectedRoom = null;

        if (_previewRoom != null)
            _previewRoom.DisablePreview();
        _previewRoom = null;

        if (_roomIndicator != null)
            Destroy(_roomIndicator);

        gridVisualization.SetActive(false);
        _inputManager.OnClicked -= PlaceRoom;
        _inputManager.OnExit -= StopPlacement;

    }

    private void Update()
    {
        if ( _selectedRoom == null )
            return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell( mousePosition );

        _roomIndicator.transform.position = _grid.CellToWorld( gridPosition );

        Bounds localBounds = _grid.GetBoundsLocal(gridPosition, _selectedRoom.roomSize);

        if ( !IsPositionFree( localBounds ) )
        {
            _previewRoom.EnablePreview();
        }
        else
        {
            if ( IsRoomNearby( localBounds, gridPosition.y, _selectedRoom.roomSize.y, out string dir ) )
            {
                _previewRoom.DisablePreview();
            } else
            {
                _previewRoom.EnablePreview();
            }

        }
    }

    private bool IsPositionFree( Bounds newRoomBounds )
    {
        newRoomBounds.Expand(-0.1f);
        foreach (Room room in _hotel.rooms)
        {
            Bounds existingRoomBounds = _grid.GetBoundsLocal( room.positionInGrid, room.roomType.roomSize );
            if ( newRoomBounds.Intersects( existingRoomBounds ) )
                return false;
        }
        return true;
    }

    private bool IsRoomNearby(Bounds newRoomBounds, int y, int roomSizeY, out string direction)
    {
        Dictionary<string, Bounds> directionalBounds = GetDirectionalBounds(newRoomBounds);

        bool negatif = y < 0;
        string position = y < 0 ? "top" : "bottom";

        if ( negatif == true )
        {
            if ((y / 3) < FindStageLevel(negatif))
            {
                direction = "";
                return false;
            }
        }
        else
        {
            if ((y / 3) > FindStageLevel(negatif))
            {
                direction = "";
                return false;
            }
        }

        int mask = 0;

        foreach (Room room in _hotel.rooms)
        {
            GameObject gameObject = GameObject.Find(room.roomID);

            Bounds existingRoomBounds = new Bounds(gameObject.transform.TransformPoint( gameObject.GetComponent<BoxCollider>().center ), gameObject.GetComponent<BoxCollider>().size);

            if (GetSubStage(y) == "GROUND" )
            {
                if (directionalBounds["right"].Intersects(existingRoomBounds))
                {
                    mask += 1; 
                }else if (directionalBounds["left"].Intersects(existingRoomBounds))
                {
                    mask += 4;
                }
            }
            else
            {
                if ( directionalBounds["right"].Intersects(existingRoomBounds ))
                {
                    mask += 1;
                }
                else if (directionalBounds["left"].Intersects(existingRoomBounds))
                {
                    mask += 4;

                } else if (directionalBounds["bottom"].Intersects(existingRoomBounds))
                {
                    mask += 8;

                }else if (directionalBounds["top"].Intersects(existingRoomBounds))
                {
                    mask += 2;
                }
            }
        }

        Debug.Log(mask);
        
        if ( GetSubStage(y) == "GROUND" && !negatif )
        {
            if (mask == 1)
            {
                direction = "right";
                return true;

            }
            else if (mask == 4)
            {
                direction = "left";
                return true;
            }
        }
        else if ( !negatif )
        {
            if ( mask == 12 )
            {
                direction = "left";
                return true;
            }else if ( mask == 9 )
            {
                direction = "right";
                return true;
            }
        }

        if ( negatif )
        {
            if (mask == 6)
            {
                direction = "left";
                return true;
            }else if (mask == 3)
            {
                direction = "right";
                return true;
            }
        }

        direction = "";

        return false;
    }

    private string IntersectsAxisX(Dictionary<string, Bounds> directionalBounds, Bounds existingRoomBounds )
    {
        string direction = "";

        if (directionalBounds["right"].Intersects(existingRoomBounds))
        {
            direction = "right";
        }
        else if (directionalBounds["left"].Intersects(existingRoomBounds))
        {
            direction = "left";
        }else if (directionalBounds["left"].Intersects(existingRoomBounds) && directionalBounds["right"].Intersects(existingRoomBounds))
        {
            direction = "both";
        }

        return direction;
    }

    private string GetStage(int y)
    {
        if (y < 0)
            return "UNDERGROUND";
        else
            return "UPPERGROUND";
    }

    private string GetSubStage(int y)
    {
        if (y < 1 && y > -3)
            return "GROUND";
        else
        {
            return "NONE";
        }
    }

    private Dictionary<string, Bounds> GetDirectionalBounds(Bounds bounds)
    {
        return new Dictionary<string, Bounds>
        {
            {"top", new Bounds(
                new Vector3(bounds.center.x,  (bounds.size.z / 2f) + bounds.center.z, bounds.center.y),
                new Vector3(0.5f, 0.5f, 0.5f)
                )},
            {"bottom", new Bounds(
                new Vector3(bounds.center.x,  bounds.center.z - (bounds.size.z / 2f), bounds.center.y),
                new Vector3(0.5f, 0.5f, 0.5f)
                )},
            {"left", new Bounds(
                new Vector3(bounds.center.x - (bounds.size.z / 1.3f), bounds.center.z, bounds.center.y),
                new Vector3(0.5f, 0.5f, 0.5f)
                )},
            {"right", new Bounds(
                new Vector3(bounds.center.x + (bounds.size.z / 1.3f), bounds.center.z, bounds.center.y),
                new Vector3(0.5f, 0.5f, 0.5f)
                )}
        };
    }

    public void AddStage( bool negatif )
    {

        List<Room> baseRooms = _hotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        if (baseRooms.Count == 0)
        {
            Console.WriteLine("Aucune pièce de type BASE trouvée.");
            return;
        }

        int newPosY = 0;
        int level = 0;

        if (negatif)
        {
            level = baseRooms.Min(x => x.level) + -1;
            newPosY = -_dataBase.roomSize.y * Math.Abs(level);

        }
        else
        {
            level = baseRooms.Max(x => x.level) + 1;
            newPosY = _dataBase.roomSize.y * level;
        }

        Vector3Int position = new Vector3Int( 0, newPosY, 0 );

        GameObject roomInstance = Instantiate(_dataBase.prefab, position, Quaternion.identity);
        roomInstance.name = roomInstance.GetInstanceID().ToString();

        _hotel.rooms.Add(new Room(_dataBase, position, roomInstance.name, level ));
    }

    private int FindStageLevel(bool negatif)
    {
        List<Room> baseRooms = _hotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        if (negatif)
        {
            return baseRooms.Min(x => x.level);

        }
        else
        {
            return baseRooms.Max(x => x.level);
        }

    }
}
