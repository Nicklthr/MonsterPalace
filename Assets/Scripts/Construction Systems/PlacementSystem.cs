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

    [SerializeField]
    private GameObject gridVisualization;

    private GridData _wallData, _roomData;
    private Renderer _previewRenderer;
    private List<GameObject> placedRooms = new();

    private void Start()
    {
        _cellIndicator.SetActive(true);

        _cellIndicator.transform.position = _grid.CellToWorld(new Vector3Int(1,1,1));


        StopPlacement();
        _wallData = new();
        _roomData = new();

        _previewRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement( int roomIndex )
    {
        StopPlacement();
        selectedRoomIndex = _roomsDataBase.roomsData.FindIndex( data => data.ID == roomIndex );
        if (selectedRoomIndex < 0 )
        {
            Debug.LogError( "No roomIndex found" );
            return;
        }

        gridVisualization.SetActive( true );
        _cellIndicator.SetActive( true );
        _inputManager.OnClicked += PlaceRoom;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceRoom()
    {
        if (_inputManager.IsPointerOverUI() )
            return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedRoomIndex);
        if ( placementValidity == false )
            return;

        GameObject newRoom = Instantiate( _roomsDataBase.roomsData[selectedRoomIndex].prefab);
        newRoom.transform.position = _grid.CellToWorld(gridPosition);
        placedRooms.Add( newRoom );

        GridData selectedData = _roomsDataBase.roomsData[selectedRoomIndex].ID == 0 ? _wallData : _roomData;

        selectedData.AddRoomAt(gridPosition,
                                _roomsDataBase.roomsData[selectedRoomIndex].roomSize,
                                _roomsDataBase.roomsData[selectedRoomIndex].ID,
                                placedRooms.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedRoomIndex)
    {
        GridData selectedData = _roomsDataBase.roomsData[selectedRoomIndex].ID == 0 ? _wallData : _roomData;

        return selectedData.CanPlaceRoomAt(gridPosition, _roomsDataBase.roomsData[selectedRoomIndex].roomSize);

    }

    private void StopPlacement()
    {
        selectedRoomIndex = -1;

        gridVisualization.SetActive( false );
        _cellIndicator.SetActive( false );
        _inputManager.OnClicked -= PlaceRoom;
        _inputManager.OnExit -= StopPlacement;

    }

    private void Update()
    {
        if ( selectedRoomIndex < 0 )
            return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell( mousePosition );

        Debug.Log( gridPosition );

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedRoomIndex );
        _previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        _mouseIndicator.transform.position = mousePosition;
        _cellIndicator.transform.position = _grid.GetCellCenterWorld( gridPosition );
    }
}
