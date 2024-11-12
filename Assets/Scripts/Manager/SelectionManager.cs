using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public InputAction mouseAction;
    public InputAction selectionAction;

    [SerializeField] private LayerMask monsterLayerMask;
    [SerializeField] private LayerMask roomLayerMask;

    private ISelectable _hoveredObject;
    private ISelectable _selectedObject;

    private PlacementSystem _placementSystem;

    public event Action<ISelectable> OnSelected;
    public event Action<ISelectable> OnDeselected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        if (mouseAction == null)
        {
            mouseAction = new InputAction(binding: "<Mouse>/position");
        }

        if (selectionAction == null)
        {
            selectionAction = new InputAction(binding: "<Mouse>/leftButton");
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

        RaycastHit hit;

        // Essayez d'abord de sélectionner un monstre
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterLayerMask))
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
        // Sinon, essayez de sélectionner une pièce
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, roomLayerMask))
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