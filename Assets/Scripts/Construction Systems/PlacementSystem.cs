using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private SO_RoomType _selectedRoom;

    [SerializeField]
    private GameObject gridVisualization;

    private GameObject _roomIndicator;
    private PreviewRoom _previewRoom;

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
        if (_selectedRoom == null)
            return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        //_mouseIndicator.transform.position = mousePosition;
        _roomIndicator.transform.position = _grid.CellToWorld(gridPosition);

        Bounds localBounds = _grid.GetBoundsLocal(gridPosition, _selectedRoom.roomSize);

        Debug.DrawLine(localBounds.min, new Vector3(localBounds.min.x, localBounds.min.y, localBounds.max.z), Color.red);

        if (!IsPositionFree(localBounds))
        {
            _previewRoom.EnablePreview();
        }
        else
        {
            if (IsRoomNearby(localBounds, gridPosition.y, _selectedRoom.roomSize.y, out string dir))
            {
                _previewRoom.DisablePreview();
            } else
            {
                _previewRoom.EnablePreview();
            }

        }
    }

    private bool IsPositionFree(Bounds newRoomBounds)
    {
        newRoomBounds.Expand(-0.1f);
        foreach (Room room in _hotel.rooms)
        {
            Bounds existingRoomBounds = _grid.GetBoundsLocal(room.positionInGrid, room.roomType.roomSize);
            if (newRoomBounds.Intersects(existingRoomBounds))
                return false;
        }
        return true;
    }

    private bool IsRoomNearby(Bounds newRoomBounds, int y, int roomSizeY, out string direction)
    {
        string stage = GetStage(y);
        Dictionary<string, Bounds> directionalBounds = GetDirectionalBounds(newRoomBounds);

        if (stage == "UNDERGROUND")
        {
            directionalBounds["top"].Expand(0.1f);
            foreach (Room room in _hotel.rooms)
            {
                Bounds existingRoomBounds = _grid.GetBoundsLocal(room.positionInGrid, room.roomType.roomSize);
                if (directionalBounds["top"].Intersects(existingRoomBounds) && directionalBounds["right"].Intersects(existingRoomBounds) && directionalBounds["left"].Intersects(existingRoomBounds))
                {
                    direction = IntersectsAxisX(directionalBounds, existingRoomBounds);
                    return true;
                }
            }
        } else
        {
            if (stage == "GROUND")
            {
                Debug.Log("Room is on the ground");

                foreach (Room room in _hotel.rooms)
                {
                    Bounds existingRoomBounds = _grid.GetBoundsLocal(room.positionInGrid, room.roomType.roomSize);
                    if (directionalBounds["right"].Intersects(existingRoomBounds) || directionalBounds["left"].Intersects(existingRoomBounds))
                    {
                        direction = IntersectsAxisX(directionalBounds, existingRoomBounds);
                        return true;
                    }
                }
            }
            else {

                Debug.Log("Room is on the top");

                directionalBounds["bottom"].Expand(0.1f);
                foreach (Room room in _hotel.rooms)
                {
                    Bounds existingRoomBounds = _grid.GetBoundsLocal(room.positionInGrid, room.roomType.roomSize);
                    if (directionalBounds["bottom"].Intersects(existingRoomBounds) && directionalBounds["right"].Intersects(existingRoomBounds) && directionalBounds["left"].Intersects(existingRoomBounds))
                    {
                        direction = IntersectsAxisX(directionalBounds, existingRoomBounds);
                        return true;
                    }
                }
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
        return y == 0 ? "GROUND" : y < 0 ? "UNDERGROUND" : "UPPERGROUND";
    }

    private Dictionary<string, Bounds> GetDirectionalBounds(Bounds bounds)
    {
        return new Dictionary<string, Bounds>
        {
            {"top", new Bounds(new Vector3(bounds.center.x, bounds.center.y + bounds.size.y / 2, bounds.center.z), new Vector3(bounds.size.x, 0.1f, bounds.size.z))},
            {"bottom", new Bounds(new Vector3(bounds.center.x, bounds.center.y - bounds.size.y / 2, bounds.center.z), new Vector3(bounds.size.x, 0.1f, bounds.size.z))},
            {"left", new Bounds(new Vector3(bounds.center.x - bounds.size.x / 2, bounds.center.y, bounds.center.z), new Vector3(0.1f, bounds.size.y, bounds.size.z))},
            {"right", new Bounds(new Vector3(bounds.center.x + bounds.size.x / 2, bounds.center.y, bounds.center.z), new Vector3(0.1f, bounds.size.y, bounds.size.z))}
        };
    }
 }
