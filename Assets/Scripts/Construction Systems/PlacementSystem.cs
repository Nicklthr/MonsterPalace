using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject _mouseIndicator;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid _grid;
    [SerializeField] private SO_RoomType _dataBase;
    [SerializeField] private SO_RoomType _stairRoom;
    [SerializeField] private GameObject gridVisualization;

    private SO_RoomType _selectedRoom;
    private GameObject _roomIndicator;
    private PreviewRoom _previewRoom;
    private MoneyManager _moneyManager;

    public event Action OnRoomPlaced, OnStairPlaced;
    public UnityEvent OnRoomBuild = new UnityEvent();
    public UnityEvent OnStairBuild = new UnityEvent();
    public UnityEvent OnNoEnoughMoney = new UnityEvent();
    public bool IsPlacingRoom => _selectedRoom != null;

    private void Start()
    {
        _mouseIndicator.SetActive(false);
        _moneyManager = FindObjectOfType<MoneyManager>();
    }

    private void Update()
    {
        if (_selectedRoom == null) return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
        gridPosition = HotelController.Instance.AdjustPositionToLevel(gridPosition);

        _roomIndicator.transform.position = _grid.CellToWorld(gridPosition);

        bool isValidPlacement = HotelController.Instance.CanPlaceRoomAt(gridPosition, new Vector2Int(_selectedRoom.roomSize.x, _selectedRoom.roomSize.y));

        if (isValidPlacement)
        {
            _previewRoom.DisablePreview();
        }
        else
        {
            _previewRoom.EnablePreview();
        }
    }

    public void StartPlacement(SO_RoomType room)
    {
        _selectedRoom = room;
        gridVisualization.SetActive(true);

        _roomIndicator = Instantiate(_selectedRoom.prefab, _mouseIndicator.transform.position, Quaternion.identity);
        _previewRoom = _roomIndicator.GetComponentInChildren<PreviewRoom>();
        if (_previewRoom == null)
        {
            _previewRoom = _roomIndicator.AddComponent<PreviewRoom>();
        }

        _inputManager.OnClicked += HandlePlaceRoom;
        _inputManager.OnExit += StopPlacement;
    }

    private void HandlePlaceRoom()
    {
        if (_selectedRoom != null)
        {
            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            gridPosition = HotelController.Instance.AdjustPositionToLevel(gridPosition);
            PlaceRoom(gridPosition);
        }
    }

    private void PlaceRoom(Vector3Int gridPosition)
    {
        if (_inputManager.IsPointerOverUI() || !CheckIfEnoughMoney(_selectedRoom.cost)) return;

        if (HotelController.Instance.CanPlaceRoomAt(gridPosition, new Vector2Int(_selectedRoom.roomSize.x, _selectedRoom.roomSize.y)))
        {
            HotelController.Instance.AddNewRoom(_selectedRoom, gridPosition);

            _moneyManager.PayRoom(_selectedRoom.cost);

            OnRoomPlaced?.Invoke();
            OnRoomBuild.Invoke();

            StopPlacement(); // Stop placement after successfully placing a room
        }
    }

    public void CancelPlacement()
    {
        StopPlacement();
    }

    private void StopPlacement()
    {
        _selectedRoom = null;

        if (_previewRoom != null)
        {
            _previewRoom.DisablePreview();
        }

        if (_roomIndicator != null)
        {
            Destroy(_roomIndicator);
        }

        _inputManager.OnClicked -= HandlePlaceRoom;
        _inputManager.OnExit -= StopPlacement;

        gridVisualization.SetActive(false);
    }

    private bool CheckIfEnoughMoney(float cost)
    {
        if (_moneyManager.playerMoney < cost)
        {
            OnNoEnoughMoney.Invoke();
            return false;
        }
        return true;
    }
}