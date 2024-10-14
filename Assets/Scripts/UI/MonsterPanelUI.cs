using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject _commentPanel;

    [Header("References")]
    [Space(10)]
    [SerializeField] private AvailableRoomsPanelUI _availableRoomsPanelUI;

    [Header("Data")]
    [Space(10)]
    [SerializeField] private SO_Hotel _hotel;

    [SerializeField] private TextMeshProUGUI _monsterName;
    [SerializeField] private Image _monsterPic;
    [SerializeField] private TextMeshProUGUI _monsterStayDay;
    [SerializeField] private MonsterController _monsterController;
    [SerializeField] private CameraController _cameraController;

    public event Action OnMonsterPanelOpen, OnMonsterPanelClose;

    private void OnEnable()
    {
        MonsterController.OnNewCommentaire += UpdateMonsterCommentaire;
        SelectionManager.Instance.OnSelected += HandleSelection;
        SelectionManager.Instance.OnDeselected += HandleDeselection;
    }

    private void OnDisable()
    {
        MonsterController.OnNewCommentaire -= UpdateMonsterCommentaire;
        SelectionManager.Instance.OnSelected -= HandleSelection;
        SelectionManager.Instance.OnDeselected -= HandleDeselection;
    }

    public void Start()
    {
        if (_monsterPanel == null)
        {
            Debug.LogError("MonsterPanelUI: Monster Panel is not assigned");
            return;
        }

        _monsterPanel.SetActive(false);
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        UpdateMonsterPanel();
    }

    private void HandleSelection(ISelectable selectable)
    {
        if (selectable is MonsterController selectedMonster)
        {
            ShowMonsterPanel(selectedMonster);
        }
    }

    private void HandleDeselection(ISelectable selectable)
    {
        if (selectable is MonsterController)
        {
            HideMonsterPanel();
        }
    }

    private void ShowMonsterPanel(MonsterController monster)
    {
        _monsterController = monster;
        OnMonsterPanelOpen?.Invoke();
        _commentPanel.SetActive(true);
        _monsterPanel.SetActive(true);
    }

    private void UpdateMonsterPanel()
    {
        if (_monsterController == null)
        {
            return;
        }

        _monsterName.text = _monsterController.monsterName;
        _monsterPic.sprite = _monsterController.monsterDatas.monsterSprite;
        _monsterStayDay.text = $"{_monsterController.currentStayDuration}/{_monsterController.stayDuration}";

        UpdateCommentPanel();
        UpdateRoomAssignment();
    }

    private void UpdateCommentPanel()
    {
        if (_monsterController.commentaries.Count > 0)
        {
            _commentPanel.SetActive(true);
            TextMeshFader.Instance.FadeTextWithUpdate(_commentPanel.GetComponentInChildren<TextMeshProUGUI>(), _monsterController.commentaries.Last());
        }
        else
        {
            _commentPanel.SetActive(false);
        }
    }

    private void UpdateRoomAssignment()
    {
        if (_monsterController.canAssignRoom)
        {
            HandleCanAssignRoom();
        }
        else
        {
            HandleCannotAssignRoom();
        }
    }

    private void HandleCanAssignRoom()
    {
        if (HasBedroom(_monsterController.monsterID))
        {
            ShowAssignedRoom();
        }
        else
        {
            ShowAvailableRooms();
        }
    }

    private void HandleCannotAssignRoom()
    {
        if (HasBedroom(_monsterController.monsterID))
        {
            ShowAssignedRoom();
        }
        else
        {
            _canAssignRoomYet.SetActive(true);
            _availableRooms.SetActive(false);
            _noRoomPanel.SetActive(false);
            _roomPanel.SetActive(false);
        }
    }

    private void ShowAssignedRoom()
    {
        _noRoomPanel.SetActive(false);
        _canAssignRoomYet.SetActive(false);
        _roomPanel.SetActive(true);
        Room assignedRoom = HotelController.Instance.GetRoomByMonsterID(_monsterController.monsterID);
        _roomPanel.GetComponentInChildren<TextTraduction>().AssignID("roomname_" + assignedRoom.roomName);

        Image roomImage = _roomPanel.transform.Find("Picto").GetComponent<Image>();
        roomImage.sprite = assignedRoom.roomType.roomSprite;

        // Set up camera movement to room
        GameObject roomObject = GameObject.Find(assignedRoom.roomID);
        Vector3 position = new Vector3(roomObject.transform.position.x + (assignedRoom.roomSize.x / 2), roomObject.transform.position.y + (assignedRoom.roomSize.y / 2), 0);
        _roomPanel.GetComponentInChildren<Button>().onClick.AddListener(() => _cameraController.MoveToTarget(position));

        _availableRooms.SetActive(false);
    }

    private void ShowAvailableRooms()
    {
        _roomPanel.SetActive(false);
        _noRoomPanel.SetActive(true);

        if (_availableRoomsPanelUI.HasBuildedRoomsInHotel())
        {
            _noRoomPanel.GetComponentInChildren<TextTraduction>().AssignID("monsterpanel_chooseroom");
        }
        else
        {
            _noRoomPanel.GetComponentInChildren<TextTraduction>().AssignID("monsterpanel_buildroom");
        }

        _availableRooms.SetActive(true);
        _canAssignRoomYet.SetActive(false);
    }

    private void UpdateMonsterCommentaire(MonsterController monster)
    {
        if (monster == _monsterController && monster.commentaries.Count > 0)
        {
            _commentPanel.SetActive(true);
            TextMeshFader.Instance.FadeTextWithUpdate(_commentPanel.GetComponentInChildren<TextMeshProUGUI>(), monster.commentaries.Last());
        }
    }

    private void HideMonsterPanel()
    {
        _monsterPanel.SetActive(false);
        _canAssignRoomYet.SetActive(false);
        _availableRooms.SetActive(false);
        _noRoomPanel.SetActive(false);
        _roomPanel.SetActive(false);

        _monsterController = null;
        OnMonsterPanelClose?.Invoke();
    }

    public bool HasBedroom(string monsterID)
    {
        List<Room> bedrooms = _hotel.rooms.FindAll(room => room.roomType.roomType == RoomType.BEDROOM && room.monsterID == monsterID);
        return bedrooms.Count > 0;
    }

    public void AddMonsterToRoom(Room room)
    {
        if (room.CheckInMonster(_monsterController))
        {
            HideMonsterPanel();
            _availableRoomsPanelUI.UpdateRoomsList();
            UpdateMonsterPanel();
            ShowMonsterPanel(_monsterController);
        }
    }
}