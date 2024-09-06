using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMonster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        Camera camera = Camera.main;

        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }

}
