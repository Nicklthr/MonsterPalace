using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private RoomSelectionManager _roomSelectionManager;
    [SerializeField] private SO_Hotel _hotel;
    [SerializeField] private Room _room;

    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private GameObject _freeRoom;
    [SerializeField] private GameObject _customerRow;
    [SerializeField] private GameObject _roomInformations;
    [SerializeField] private GameObject _NofoodRow;
    [SerializeField] private GameObject _foodRow;

    [Space(10)]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Transform _foodGrid;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private List<SO_Food> _foods;

    private void Start()
    {
        _roomSelectionManager = FindObjectOfType<RoomSelectionManager>();
        _roomPanel.SetActive(false);
        _roomSelectionManager.OnSelectedRoom += ShowRoomPanel;
        _roomSelectionManager.OnDeSelectedRoom += HideRoomPanel;

        _NofoodRow.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
           MenuPanel();
        });
    }

    private void ShowRoomPanel()
    {
        FindRoomInHotel();

        _roomName.text = _room.roomName;
        
        switch( _room.type )
            {
            case RoomType.BEDROOM:
                HandleRoom();
                break;
            default:
                HandleOtherRoom();
                break;
        }

        _roomPanel.SetActive(true);
    }

    private void HideRoomPanel()
    {
        _room = null;
        _roomPanel.SetActive(false);
    }

    private void FindRoomInHotel()
    {
       foreach ( Room room in _hotel.rooms )
        {
            if ( room.roomID == _roomSelectionManager._selectedRoom.name )
            {
                _room = room;
            }
        }
    }

    private void HandleRoom()
    {
        if ( _room.monsterID == null )
        {
            _freeRoom.SetActive(true);
            _customerRow.SetActive(false);
            _roomInformations.SetActive(false);
        }
        else
        {
            _roomInformations.SetActive(false);
            _freeRoom.SetActive(false);
            _customerRow.SetActive(true);

            MonsterController monster = GameObject.Find( _room.monsterID ).GetComponent<MonsterController>();

            if( monster != null )
            {
                _customerRow.GetComponentInChildren<TextMeshProUGUI>().text = monster.monsterName;
            }

            if(_room.foodAssigned == null)
            {
                _NofoodRow.SetActive(true);
                _foodRow.SetActive(false);
            }
            else
            {
                _NofoodRow.SetActive(false);
                _foodRow.SetActive(true);

                _foodRow.GetComponentInChildren<TextMeshProUGUI>().text = "Repas : " + _room.foodAssigned.foodName;
            }
        }
    }

    private void HandleOtherRoom()
    {
        _freeRoom.SetActive(false);
        _customerRow.SetActive(false);
        _roomInformations.SetActive(true);
    }

    public void MenuPanel()
    {

        _menuPanel.SetActive(true);

        foreach ( Transform child in _foodGrid )
        {
            Destroy(child.gameObject);
        }

        foreach ( SO_Food food in _foods )
        {
            GameObject card = Instantiate(_cardPrefab, _foodGrid);

            card.GetComponent<CardFoodUI>().SetFood( food );
            card.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                SetRoomFood( food );
           });
        }

    }

    public void SetRoomFood( SO_Food food )
    {
        _room.foodAssigned = food;
        _menuPanel.SetActive( false );
        HandleRoom();
    }
}
