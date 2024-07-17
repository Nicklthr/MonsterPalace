using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

public class MonsterSelectionManager : MonoBehaviour
{
    public InputAction _mouse;
    public InputAction _selectionMonster;
    public Transform _selectedMonster;
    [SerializeField]
    private Transform _hoverMonster;
    [SerializeField]
    private LayerMask selectibleMask;

    public event Action OnSelected, OnDeSelected;

    private void OnEnable()
    {
        _mouse.Enable();
        _selectionMonster.Enable();
    }

    void Update()
    {
        Vector2 mousePosition = _mouse.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        if ( Physics.Raycast(ray, out hit, Mathf.Infinity, selectibleMask ) )
        {
            if ( hit.transform.CompareTag("Monster") )
            {

                if ( _hoverMonster != hit.transform )
                {
                    HoverOutMonster();

                    _hoverMonster = hit.transform;

                    Outline outline = _hoverMonster.GetComponent<Outline>();
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
        }else
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
        if ( _hoverMonster != null )
        {
            Outline outline = _hoverMonster.GetComponent<Outline>();
            if ( outline != null )
            {
                if( _hoverMonster != _selectedMonster)
                {
                    outline.enabled = false;
                }
            }

            _hoverMonster = null;
        }
    }

    private void SelectMonster()
    {
       if ( _selectionMonster.WasPerformedThisFrame() && _hoverMonster != null )
        {
            DeselectMonster();

            Outline outline = _hoverMonster.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
            _selectedMonster = _hoverMonster;
            OnSelected?.Invoke();
            _hoverMonster = null;
        }
    }

    private void DeselectMonster()
    {
        if ( _selectionMonster.WasPerformedThisFrame() && _selectedMonster != null )
        {
            OnDeSelected?.Invoke();
            Outline outline = _selectedMonster.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            _selectedMonster = null;
        }
    }

}
