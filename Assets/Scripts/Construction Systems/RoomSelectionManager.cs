using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RoomSelectionManager : MonoBehaviour
{
    public InputAction _mouse;
    public InputAction _selectionRoom;
    public Transform _selectedRoom;
    [SerializeField]
    private Transform _hoverRoom;
    [SerializeField]
    private LayerMask selectibleMask;

    public event Action OnSelectedRoom, OnDeSelectedRoom;

    private PlacementSystem _placementSystem;
    private MonsterSelectionManager _monsterSelectionManager;

    private void OnEnable()
    {
        _mouse.Enable();
        _selectionRoom.Enable();
    }

    private void Start()
    {
        _placementSystem = FindObjectOfType<PlacementSystem>();
        _monsterSelectionManager = FindObjectOfType<MonsterSelectionManager>();
    }

    void Update()
    {
        if ( IsPointerOverUI() )
        {
            return;
        }

        if ( _placementSystem.IsPlacingRoom )
        {
            return;
        }

        if ( _monsterSelectionManager._hoverMonster != null )
        {
            HoverOutRoom();
            DeselectRoom();
            return;
        }

        Vector2 mousePosition = _mouse.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectibleMask))
        {
            if (hit.transform.CompareTag("Room"))
            {

                if (_hoverRoom != hit.transform)
                {
                    HoverOutRoom();

                    _hoverRoom = hit.transform;

                    OutlineRoom outline = _hoverRoom.GetComponent<OutlineRoom>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                }
            }
            else
            {
                HoverOutRoom();
            }
        }
        else
        {
            HoverOutRoom();
        }

        if (!IsPointerOverUI())
        {

            DeselectRoom();
        }

        SelectRoom();
    }


    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    private void HoverOutRoom()
    {
        if (_hoverRoom != null)
        {
            OutlineRoom outline = _hoverRoom.GetComponent<OutlineRoom>();
            if (outline != null)
            {
                if ( _hoverRoom != _selectedRoom )
                {
                    outline.enabled = false;
                }
            }

            _hoverRoom = null;
        }
    }

    private void SelectRoom()
    {
        if ( _selectionRoom.WasPerformedThisFrame() && _hoverRoom != null )
        {
            DeselectRoom();

            OutlineRoom outline = _hoverRoom.GetComponent<OutlineRoom>();
            if (outline != null)
            {
                outline.enabled = true;
            }
            _selectedRoom = _hoverRoom;
            OnSelectedRoom?.Invoke();
            _hoverRoom = null;
        }
    }

    private void DeselectRoom()
    {
        if ( _selectionRoom.WasPerformedThisFrame() && _selectedRoom != null )
        {
            OnDeSelectedRoom?.Invoke();
            OutlineRoom outline = _selectedRoom.GetComponent<OutlineRoom>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            _selectedRoom = null;
        }
    }
}
