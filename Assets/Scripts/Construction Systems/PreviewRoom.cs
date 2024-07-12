using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewRoom : MonoBehaviour
{
    [SerializeField]
    private GameObject _previewCube;

    public void EnablePreview()
    {
        _previewCube.SetActive(true);
    }

    public void DisablePreview()
    {
        _previewCube.SetActive(false);
    }
}

