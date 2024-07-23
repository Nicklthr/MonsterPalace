using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPanelUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject _monsterPanel;

    [Header("Room Panel")]
    [Space(10)]
    [SerializeField] private GameObject _noRoomPanel;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _availableRooms;
    [SerializeField] private GameObject _canAssignRoomYet;

    [Header("References")]
    [Space(10)]
    [SerializeField] private AvailableRoomsPanelUI _availableRoomsPanelUI;
    [SerializeField] private MonsterSelectionManager _monsterSelectionManager;

    [Header("Data")]
    [Space(10)]
    [SerializeField] private SO_Hotel _hotel;

    [SerializeField] private TextMeshProUGUI _monsterName;
    [SerializeField] private Image _monsterPic;
    [SerializeField] private TextMeshProUGUI _monsterStayDay;
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

        _monsterPic.sprite = _monsterController.monsterDatas.monsterSprite;

        _monsterStayDay.text = _monsterController.currentStayDuration.ToString() + "/" + _monsterController.stayDuration.ToString() + " Jours";

        if (!_monsterController.canAssignRoom)
        {
            if( HasBedroom(_monsterController.monsterID))
            {
                _noRoomPanel.SetActive(false);
                _roomPanel.SetActive(true);
                _roomPanel.GetComponentInChildren<TextMeshProUGUI>().text = _hotel.rooms.Find(room => room.monsterID == _monsterController.monsterID).roomName;

                _availableRooms.SetActive(false);
                _canAssignRoomYet.SetActive(false);
            }
            else
            {
                _canAssignRoomYet.SetActive(true);
                _availableRooms.SetActive(false);
                _noRoomPanel.SetActive(false);
                _roomPanel.SetActive(false);
            }
        }
        else
        {
            if (!HasBedroom(_monsterController.monsterID))
            {
                _roomPanel.SetActive(false);
                _noRoomPanel.SetActive(true);
                _availableRooms.SetActive(true);
                _canAssignRoomYet.SetActive(false);
            }
            else
            {
                _noRoomPanel.SetActive(false);
                _canAssignRoomYet.SetActive(false);
                _roomPanel.SetActive(true);
                _roomPanel.GetComponentInChildren<TextMeshProUGUI>().text = _hotel.rooms.Find(room => room.monsterID == _monsterController.monsterID).roomName;

                _availableRooms.SetActive(false);
            }
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

        if ( _monsterController.canAssignRoom )
        {
            if( room.currentUsers < room.roomType.maxUsers )
            {
                room.monsterID = _monsterController.monsterID;
                room.monsterDataCurrentCustomer = _monsterController.monsterDatas;
                room.currentUsers++;

                TargetInRoom targetInRoom = room.targets.FindLast(target => target.isOccupied == false);


                if (targetInRoom != null)
                {
                    _monsterController.roomPosition = targetInRoom.target;
                }else
                {
                    Debug.LogError( "MonsterPanelUI: No target available" );
                }

                _monsterController.roomAssigned = true;

                GameObject _roomObject = GameObject.Find( room.roomID );
                _roomObject.GetComponent<RoomController>().ToggleLights();

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
