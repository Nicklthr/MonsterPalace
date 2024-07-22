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

    private void OnEnable()
    {
        _mouse.Enable();
        _selectionRoom.Enable();
    }

    void Update()
    {
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
                    HoverOutMonster();

                    _hoverRoom = hit.transform;

                    Outline outline = _hoverRoom.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                }
            }
            else
            {
                HoverOutMonster();
            }
        }
        else
        {
            HoverOutMonster();
        }

        if (!IsPointerOverUI())
        {

            DeselectMonster();
        }

        SelectMonster();
    }


    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    private void HoverOutMonster()
    {
        if (_hoverRoom != null)
        {
            Outline outline = _hoverRoom.GetComponent<Outline>();
            if (outline != null)
            {
                if (_hoverRoom != _selectedRoom)
                {
                    outline.enabled = false;
                }
            }

            _hoverRoom = null;
        }
    }

    private void SelectMonster()
    {
        if (_selectionRoom.WasPerformedThisFrame() && _hoverRoom != null)
        {
            DeselectMonster();

            Outline outline = _hoverRoom.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
            _selectedRoom = _hoverRoom;
            OnSelectedRoom?.Invoke();
            _hoverRoom = null;
        }
    }

    private void DeselectMonster()
    {
        if ( _selectionRoom.WasPerformedThisFrame() && _selectedRoom != null )
        {
            OnDeSelectedRoom?.Invoke();
            Outline outline = _selectedRoom.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            _selectedRoom = null;
        }
    }
}
