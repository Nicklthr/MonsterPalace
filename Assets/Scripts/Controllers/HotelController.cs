using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotelController : MonoBehaviour
{
    [SerializeField]
    private SO_Hotel _hotel;
    [SerializeField]
    private SO_RoomType _dataBase;
    [SerializeField]
    private Grid _grid;
    [SerializeField]

    public UnityEvent OnHotelCreated = new UnityEvent();

    private void Start()
    {
        if ( _hotel.rooms.Count != 0 )
        {
            AddRooms();
        }
        else
        {
            AddBaseRoom();
        }
    }

    private void AddRooms()
    {
        foreach ( var room in _hotel.rooms )
        {
            GameObject prefab = room.roomType.prefab;

            GameObject roomInstance = Instantiate(prefab, room.positionInGrid, Quaternion.identity);
            roomInstance.name = room.roomID.ToString();
            roomInstance.transform.SetParent(transform);

        }

        OnHotelCreated.Invoke();
    }

    private void AddBaseRoom()
    {
        SO_RoomType baseRoom = _dataBase;

        GameObject roomInstance = Instantiate(baseRoom.prefab, Vector3.zero, Quaternion.identity);
        roomInstance.name = baseRoom.GetInstanceID().ToString();

        roomInstance.transform.SetParent(transform);

        GameObject recpetion = GameObject.FindGameObjectWithTag("ReceptionPosition");

        if ( recpetion != null )
        {
            recpetion.SetActive( true );
        }

        _hotel.rooms.Add(new Room(baseRoom, _grid.WorldToCell(Vector3Int.zero), baseRoom.GetInstanceID().ToString(), 0));
        OnHotelCreated.Invoke();
    }
}
