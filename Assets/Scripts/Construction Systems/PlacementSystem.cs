using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _mouseIndicator, _cellIndicator;
    [SerializeField]
    private InputManager _inputManager;
    [SerializeField]
    private Grid _grid;

    [SerializeField]
    private SO_RoomsDataBase _roomsDataBase;
    private int selectedRoomIndex = -1;
    private SO_RoomType selectedRoom;

    [SerializeField]
    private GameObject gridVisualization;

    private Renderer _previewRenderer;
    private GameObject _roomIndicator;

    private void Start()
    {
        StopPlacement();
        _previewRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement( SO_RoomType room )
    {
        StopPlacement();
        selectedRoom = room;

        gridVisualization.SetActive( true );
        //_cellIndicator.SetActive( true );

        _roomIndicator = Instantiate( selectedRoom.prefab, _cellIndicator.transform.position, Quaternion.identity );

        _inputManager.OnClicked += PlaceRoom;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceRoom()
    {
        if ( _inputManager.IsPointerOverUI() )
            return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell( mousePosition );


        GameObject newRoom = Instantiate( selectedRoom.prefab);
        newRoom.transform.position = _grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectedRoom = null;
        _roomIndicator = null;

        gridVisualization.SetActive( false );
        _cellIndicator.SetActive( false );
        _inputManager.OnClicked -= PlaceRoom;
        _inputManager.OnExit -= StopPlacement;

    }

    private void Update()
    {
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell( mousePosition );

        Debug.Log( gridPosition );

        _mouseIndicator.transform.position = mousePosition;

        _cellIndicator.transform.position = _grid.CellToWorld( gridPosition );
        _roomIndicator.transform.position = _grid.CellToWorld( gridPosition );
    }
}
