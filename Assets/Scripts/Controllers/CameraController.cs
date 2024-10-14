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

    [Header("Camera Zones")]
    [Space(10)]
    [SerializeField] private float _rightZone = 0.9f;
    [SerializeField] private float _leftZone = 0.1f;
    [SerializeField] private float _topZone = 0.98f;
    [SerializeField] private float _bottomZone = 0.02f;

    [Header("Keyboard Controls")]
    [Space(10)]
    public InputAction movement;

    private void Update()
    {
        if (isMovingToTarget)
        {
            MoveToTargetUpdate();
        }
        else
        {
            HandleMove();
            HandleKeyboardMove();
        }
        HandleZoom();
    }

    private void OnEnable()
    {
        zoom.Enable();
        movement.Enable();
    }

    private void OnDisable()
    {
        zoom.Disable();
        movement.Disable();
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

        Vector3 moveDirection = Vector3.zero;

        if (pos.x <= _leftZone && (Camera.main.transform.position.x >= _MinMaxBoundsX.x || _MinMaxBoundsX.x == 0))
        {
            moveDirection += Vector3.left;
        }
        else if (pos.x >= _rightZone && (Camera.main.transform.position.x <= _MinMaxBoundsX.y || _MinMaxBoundsX.y == 0))
        {
            moveDirection += Vector3.right;
        }

        if (pos.y <= _bottomZone && Camera.main.transform.position.y >= (HotelController.Instance.MinStage * HotelController.Instance.GetLevelHeight()))
        {
            moveDirection += Vector3.down;
        }
        else if (pos.y >= _topZone && Camera.main.transform.position.y <= ((HotelController.Instance.MaxStage * HotelController.Instance.GetLevelHeight()) + 5))
        {
            moveDirection += Vector3.up;
        }

        Camera.main.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleKeyboardMove()
    {
        Vector2 input = movement.ReadValue<Vector2>();
        Vector3 moveDirection = Vector3.zero;

        if (input.x < 0 && (Camera.main.transform.position.x >= _MinMaxBoundsX.x || _MinMaxBoundsX.x == 0))
        {
            moveDirection += Vector3.left;
        }
        else if (input.x > 0 && (Camera.main.transform.position.x <= _MinMaxBoundsX.y || _MinMaxBoundsX.y == 0))
        {
            moveDirection += Vector3.right;
        }

        if (input.y < 0 && Camera.main.transform.position.y >= (HotelController.Instance.MinStage * HotelController.Instance.GetLevelHeight()))
        {
            moveDirection += Vector3.down;
        }
        else if (input.y > 0 && Camera.main.transform.position.y <= ((HotelController.Instance.MaxStage * HotelController.Instance.GetLevelHeight()) + 5))
        {
            moveDirection += Vector3.up;
        }

        Camera.main.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private bool IsUnderGroundStage()
    {
        List<Room> baseroom = _dataHotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        return baseroom.Exists(room => room.level < 0);
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