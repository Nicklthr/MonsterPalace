using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private SO_Hotel _dataHotel;


    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint( Mouse.current.position.ReadValue() );

        if ( pos.x <= 0.1 )
        {
            Camera.main.transform.Translate( Vector3.left * moveSpeed * Time.deltaTime );
        }
        else if ( pos.x >= 0.9 )
        {
            Camera.main.transform.Translate( Vector3.right * moveSpeed * Time.deltaTime );
        }
        if  ( pos.y <= 0.1 )
        {
            if (Camera.main.transform.position.y < 2.2)
            {
                return;         
            }

            Camera.main.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        }
        else if ( pos.y >= 0.98 )
        {
            if( Camera.main.transform.position.y > GetMaxY() )
            {
                return;
            }

            Camera.main.transform.Translate( Vector3.up * moveSpeed * Time.deltaTime );
        }
    }

    private bool IsUnderGroundStage()
    {
        List<Room> baseroom = _dataHotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        return baseroom.Exists(room => room.level < 0);
    }

    private int GetMaxY()
    {
        List<Room> baseroom = _dataHotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BASE);

        int max = 0;
        foreach (Room room in baseroom)
        {
            if ( room.level > max )
            {
                max = room.level;
            }
        }

        return (max * 5) + 5;
    }

}
