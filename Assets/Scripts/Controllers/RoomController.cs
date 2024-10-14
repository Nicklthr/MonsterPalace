using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour, ISelectable
{
    [SerializeField] private Light[] _lights;
    [SerializeField] private GameObject _removableFacade;
    [SerializeField] private Room room;
    public Room Room => room;

    private OutlineRoom _outline;

    public void OnHoverEnter() => SetOutline(true);
    public void OnHoverExit() => SetOutline(false);
    public void OnSelect() => SetOutline(true);
    public void OnDeselect() => SetOutline(false);

    private void Awake()
    {
        _outline = GetComponent<OutlineRoom>();
    }

    private void Start()
    {
        if (_removableFacade == null)
        {
            Debug.LogError("RoomController: Removable facade is not assigned");
        }

        _removableFacade.SetActive( false );
    }

    public void SetRoom(Room room)
    {
        this.room = room;
    }

    private void SetOutline(bool enabled)
    {
        if (_outline != null)
        {
            _outline.enabled = enabled;
        }
    }

    public void ToggleLights()
    {
        if (_lights.Length == 0)
        {
            Debug.LogWarning("No lights found");
            return;
        }

        foreach (var light in _lights)
        {
            light.enabled = !light.enabled;
        }
    }

}
