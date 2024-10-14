using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    private static SelectionManager _instance;
    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SelectionManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SelectionManager");
                    _instance = go.AddComponent<SelectionManager>();
                }
            }
            return _instance;
        }
    }

    public InputAction mouseAction;
    public InputAction selectionAction;

    [SerializeField] private LayerMask selectableMask;

    private ISelectable _hoveredObject;
    private ISelectable _selectedObject;

    private PlacementSystem _placementSystem;

    public event Action<ISelectable> OnSelected;
    public event Action<ISelectable> OnDeselected;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        mouseAction.Enable();
        selectionAction.Enable();
    }

    private void OnDisable()
    {
        mouseAction.Disable();
        selectionAction.Disable();
    }

    private void Start()
    {
        _placementSystem = FindObjectOfType<PlacementSystem>();
    }

    private void Update()
    {
        if (IsPointerOverUI() || (_placementSystem != null && _placementSystem.IsPlacingRoom))
        {
            return;
        }

        HandleHovering();
        HandleSelection();
    }

    private void HandleHovering()
    {
        Vector2 mousePosition = mouseAction.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableMask))
        {
            ISelectable selectable = hit.transform.GetComponent<ISelectable>();
            if (selectable != null)
            {
                if (_hoveredObject != selectable)
                {
                    if (_hoveredObject != null && _hoveredObject != _selectedObject)
                    {
                        _hoveredObject.OnHoverExit();
                    }
                    _hoveredObject = selectable;
                    if (_hoveredObject != _selectedObject)
                    {
                        _hoveredObject.OnHoverEnter();
                    }
                }
            }
            else
            {
                ClearHoveredObject();
            }
        }
        else
        {
            ClearHoveredObject();
        }
    }

    private void HandleSelection()
    {
        if (selectionAction.WasPerformedThisFrame())
        {
            if (_hoveredObject != null)
            {
                if (_selectedObject == _hoveredObject)
                {
                    Deselect();
                }
                else
                {
                    Select(_hoveredObject);
                }
            }
            else
            {
                Deselect();
            }
        }
    }

    private void Select(ISelectable selectable)
    {
        if (_selectedObject != selectable)
        {
            Deselect();
            _selectedObject = selectable;
            _selectedObject.OnSelect();
            OnSelected?.Invoke(_selectedObject);
        }
    }

    private void Deselect()
    {
        if (_selectedObject != null)
        {
            _selectedObject.OnDeselect();
            OnDeselected?.Invoke(_selectedObject);
            _selectedObject = null;
        }
    }

    private void ClearHoveredObject()
    {
        if (_hoveredObject != null && _hoveredObject != _selectedObject)
        {
            _hoveredObject.OnHoverExit();
            _hoveredObject = null;
        }
    }

    private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public T GetSelectedObject<T>() where T : ISelectable
    {
        return _selectedObject is T selectedT ? selectedT : default;
    }
}

public interface ISelectable
{
    void OnHoverEnter();
    void OnHoverExit();
    void OnSelect();
    void OnDeselect();
}