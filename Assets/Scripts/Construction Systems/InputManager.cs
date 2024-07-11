using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            OnClicked?.Invoke();
        }

        if ( Input.GetKeyDown( KeyCode.Escape ) )
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        Ray ray = Camera.main.ScreenPointToRay( mousePos );
        RaycastHit hit;

        Debug.DrawRay( ray.origin, ray.direction * 1000, Color.red );

        if ( Physics.Raycast( ray, out hit, Mathf.Infinity, placementLayerMask ) )
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
