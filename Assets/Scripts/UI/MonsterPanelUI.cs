using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterPanelUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject _monsterPanel;

    [Header("Room Panel")]
    [Space(10)]
    [SerializeField] private GameObject _noRoomPanel;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _availableRooms;

    [Header("References")]
    [Space(10)]
    [SerializeField] private AvailableRoomsPanelUI _availableRoomsPanelUI;
    [SerializeField] private MonsterSelectionManager _monsterSelectionManager;

    [Header("Data")]
    [Space(10)]
    [SerializeField] private SO_Hotel _hotel;

    [SerializeField] private TextMeshProUGUI _monsterName;
    [SerializeField] private MonsterController _monsterController;


    public event Action OnOpen;

    public void Start()
    {
        if ( _monsterPanel == null )
        {
            Debug.LogError( "MonsterPanelUI: Monster Panel is not assigned" );
            return;
        }

        _monsterPanel.SetActive( false );

        _monsterSelectionManager.OnSelected += ShowMonsterPanel;
        _monsterSelectionManager.OnDeSelected += HideMonsterPanel;
    }

    private void ShowMonsterPanel()
    {
        _monsterController = null;

        OnOpen?.Invoke();

        _monsterController = _monsterSelectionManager._selectedMonster.GetComponent<MonsterController>();
        _monsterName.text = _monsterController.monsterName;

        if ( !HasBedroom( _monsterController.monsterID ) )
        {
            _roomPanel.SetActive(false);
            _noRoomPanel.SetActive(true);
            _availableRooms.SetActive( true );

        }else
        {
            _noRoomPanel.SetActive( false );
            _roomPanel.SetActive( true );
            _roomPanel.GetComponentInChildren<TextMeshProUGUI>().text = _hotel.rooms.Find(room => room.monsterID == _monsterController.monsterID).roomName;

            _availableRooms.SetActive( false );
        }

        _monsterPanel.SetActive( true );
    }

    private void HideMonsterPanel()
    {
        _monsterPanel.SetActive( false );
        _monsterController = null;
    }

    public bool HasBedroom(string monsterID)
    {
        List<Room> bedrooms = _hotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BEDROOM && room.monsterID == monsterID);

        return bedrooms.Count > 0;

    }

    public void AddMonsterToRoom( Room room )
    {
        if (_monsterController == null )
        {
            Debug.LogError( "MonsterPanelUI: MonsterController is not assigned" );
            return;
        }

        if (_monsterController.canAssignRoom)
        {
            room.monsterID = _monsterController.monsterID;
            if( room.currentUsers < room.roomType.maxUsers )
            {
                room.currentUsers++;

                _monsterController.roomPosition = GameObject.Find(room.roomID).transform;
                HideMonsterPanel();
                _availableRoomsPanelUI.UpdateRoomsList();
                ShowMonsterPanel();
            }else
            {
                Debug.LogError( "MonsterPanelUI: Room is full" );
            }
        }
    }
}
