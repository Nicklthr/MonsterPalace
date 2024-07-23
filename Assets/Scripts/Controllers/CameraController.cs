using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private SO_Hotel _dataHotel;

    public InputAction zoom;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomMin = 40f;
    [SerializeField] private float zoomMax = 140f;


    private void Update()
    {
        HandleMove();
        HandleZoom();
    }

    private void OnEnable()
    {
        zoom.Enable();
    }

    private void HandleMove()
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint( Mouse.current.position.ReadValue() );

        if ( pos.x <= 0.1 )
        {
            Camera.main.transform.Translate( Vector3.left * moveSpeed * Time.deltaTime );
        }
        else if ( pos.x >= 0.9 )
        {
            Camera.main.transform.Translate( Vector3.right * moveSpeed * Time.deltaTime );
        }
        if  ( pos.y <= 0.1 )
        {
            if (Camera.main.transform.position.y < GetMinY() )
            {
                return;         
            }

            Camera.main.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        }
        else if ( pos.y >= 0.98 )
        {
            if( Camera.main.transform.position.y > GetMaxY() )
            {
                return;
            }

            Camera.main.transform.Translate( Vector3.up * moveSpeed * Time.deltaTime );
        }
    }

    private bool IsUnderGroundStage()
    {
        List<Room> baseroom = _dataHotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        return baseroom.Exists(room => room.level < 0);
    }

    private int GetMaxY()
    {
        List<Room> baseroom = _dataHotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        int max = 0;
        foreach (Room room in baseroom)
        {
            if ( room.level > max )
            {
                max = room.level;
            }
        }

        return (max * 5) + 5;
    }

    private int GetMinY()
    {
        List<Room> baseRooms = _dataHotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        int level = 0;

        level = baseRooms.Min(x => x.level);

        return (level * 5);
    }

    private void HandleZoom()
    {

        if ( IsPointerOverUI() )
        {
            return;
        }

        float zoomValue = zoom.ReadValue<float>();

        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomValue * zoomSpeed * Time.deltaTime, zoomMin, zoomMax);
    }

    public bool IsPointerOverUI()
    => EventSystem.current.IsPointerOverGameObject();

}
