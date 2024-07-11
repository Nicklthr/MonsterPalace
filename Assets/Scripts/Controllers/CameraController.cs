using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint( Mouse.current.position.ReadValue() );

        if (pos.x <= 0.1)
        {
            Camera.main.transform.Translate( Vector3.left * moveSpeed * Time.deltaTime );
        }
        else if (pos.x >= 0.9)
        {
            Camera.main.transform.Translate( Vector3.right * moveSpeed * Time.deltaTime );
        }
        if (pos.y <= 0.1)
        {
            Camera.main.transform.Translate( Vector3.down * moveSpeed * Time.deltaTime );
        }
        else if (pos.y >= 0.98)
        {
            Camera.main.transform.Translate( Vector3.up * moveSpeed * Time.deltaTime );
        }
    }

    private void HandleZoom()
    {
    }
}
