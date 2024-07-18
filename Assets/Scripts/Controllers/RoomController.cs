using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Light[] _lights;
    [SerializeField] private GameObject _removableFacade;

    private void Start()
    {
        if (_removableFacade == null)
        {
            Debug.LogError("RoomController: Removable facade is not assigned");
        }

        _removableFacade.SetActive( false );
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
