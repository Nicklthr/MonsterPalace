using Michsky.UI.Dark;
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
    [SerializeField] private GameObject _header;
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

    private bool isDissolvingOut = false;
    private bool isDissolvingIn = false;

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
        _roomPanel.SetActive(true);
        _roomPanel.GetComponent<UIDissolveEffect>().DissolveIn();

        FindRoomInHotel();
        _header.SetActive(true);
        _roomName.text = _room.roomName;

        switch (_room.type)
        {
            case RoomType.BEDROOM:
                HandleRoom();
                break;
            default:
                HandleOtherRoom();
                break;
        }

        _roomInformations.SetActive(true);

        _roomInformations.GetComponent<RoomInformationsPanelUI>().SetRoomInformations(_room.maxUsers, _room.currentUsers);
    }

    private void HideRoomPanel()
    {
        if ( isDissolvingOut ) return;
        _room = null;
        _header.SetActive(false);
        _foodRow.SetActive(false);
        _NofoodRow.SetActive(false);
        _customerRow.SetActive(false);
        _roomInformations.SetActive(false);
        _freeRoom.SetActive(false);

        _roomPanel.GetComponent<UIDissolveEffect>().DissolveOut();
        StartCoroutine(DissolveOut());

    }

    // coroutin to wait for the dissolve effect
    public IEnumerator DissolveOut()
    {
        isDissolvingOut = true;
        yield return new WaitForSeconds(_roomPanel.GetComponent<UIDissolveEffect>().animationSpeed);
        _roomPanel.SetActive(false);
        isDissolvingOut = false;
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
                _customerRow.GetComponent<CustomerRowUI>().SetCustomerData( monster.monsterName, monster.monsterDatas.monsterSprite );
            }

            if( _room.foodAssigned == null )
            {
                _NofoodRow.SetActive( true );
                _foodRow.SetActive( false );
            }
            else
            {
                _NofoodRow.SetActive( false );
                _foodRow.SetActive( true );

                _foodRow.GetComponentInChildren<TextMeshProUGUI>().text = "Repas : " + _room.foodAssigned.foodName;
            }
        }
    }

    private void HandleOtherRoom()
    {
        _freeRoom.SetActive(false);
        _customerRow.SetActive(false);
        _foodRow.SetActive(false);
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
            if ( food.isUnlocked == false ) continue;

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
