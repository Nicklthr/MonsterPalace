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
    [Space(10)]
    [Header("Zoom")]
    public InputAction zoom;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomMin = 10.40f;
    [SerializeField] private float zoomMax = -3f;

    private Vector3 targetPosition;
    private bool isMovingToTarget = false;
    [SerializeField] private float targetMoveSpeed = 15f;

    [SerializeField] private Vector2 _MinMaxBoundsX = new Vector2(0, 0);

    private void Update()
    {
        if (isMovingToTarget)
        {
            MoveToTargetUpdate();
        }
        else
        {
            HandleMove();
        }
        HandleZoom();
    }

    private void OnEnable()
    {
        zoom.Enable();
    }

    public void MoveToTarget(Vector3 target)
    {
        targetPosition = new Vector3(target.x, target.y, transform.position.z);
        isMovingToTarget = true;
    }

    private void MoveToTargetUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, targetMoveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMovingToTarget = false;
        }
    }

    private void HandleMove()
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        if (pos.x <= 0.1)
        {
            if ( Camera.main.transform.position.x < _MinMaxBoundsX.x && _MinMaxBoundsX.x != 0)
            {
                return;
            }

            Camera.main.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (pos.x >= 0.9)
        {
            if ( Camera.main.transform.position.x > _MinMaxBoundsX.y && _MinMaxBoundsX.y != 0 )
            {
                return;
            }

            Camera.main.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (pos.y <= 0.02)
        {
            if (Camera.main.transform.position.y < GetMinY())
            {
                return;
            }
            Camera.main.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        else if (pos.y >= 0.98)
        {
            if (Camera.main.transform.position.y > GetMaxY())
            {
                return;
            }
            Camera.main.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
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
        if (IsPointerOverUI())
        {
            return;
        }
        float zoomValue = zoom.ReadValue<float>();
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                                                     Camera.main.transform.position.y,
                                                     Mathf.Clamp(Camera.main.transform.position.z + zoomValue * zoomSpeed * Time.deltaTime, zoomMax, zoomMin)
                                                    );
    }

    public bool IsPointerOverUI()
    => EventSystem.current.IsPointerOverGameObject();

}
