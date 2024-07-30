using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotelManageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bedrooms;
    [SerializeField] private TextMeshProUGUI _activityRooms;
    [SerializeField] private TextMeshProUGUI _diningRooms;

    [SerializeField] private TextMeshProUGUI _customers;
    [SerializeField] private TextMeshProUGUI _customersWating;

    [SerializeField] private SO_Hotel _dataHotel;
    [SerializeField] private PlacementSystem _placementSystem;
    [SerializeField] private WaitingQ _waitingQ;

    private void Awake()
    {
        _placementSystem = FindObjectOfType<PlacementSystem>();
    }

    void Start()
    {
        UpdateUIRooms();
        _waitingQ = FindObjectOfType<WaitingQ>();
        _placementSystem.OnRoomPlaced += UpdateUIRooms;
    }


    void Update()
    {
        _customers.text = "Customers : " + CustomerInHotel().ToString();
        _customersWating.text = "Waiting : " + CustomersWaiting().ToString();
    }

    private int RoomsInHotel(RoomType roomType)
    {
        List<Room> bedrooms = _dataHotel.rooms.FindAll(room => room.roomType.roomType == roomType);

        return bedrooms.Count;
    }

    private int CustomerInHotel()
    {
        List<Room> bedrooms = _dataHotel.rooms.FindAll(room => room.roomType.roomType != RoomType.BASE && room.monsterID != null);

        return bedrooms.Count;
    }

    public void UpdateUIRooms()
    {
        _bedrooms.text = "Bedrooms : " + RoomsInHotel(RoomType.BEDROOM).ToString();
        _activityRooms.text = "Activity : " + RoomsInHotel(RoomType.ACTIVITY).ToString();
        _diningRooms.text = "Dining : " + RoomsInHotel(RoomType.DINING).ToString();
    }

    private int CustomersWaiting()
    {
        return WaitingQ.instance.waiting.FindAll(spot => spot.monster != null).Count;
    }
}
