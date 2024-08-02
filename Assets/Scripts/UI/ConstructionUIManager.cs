using Michsky.UI.Dark;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionUIManager : MonoBehaviour
{
    public SO_RoomsDB data;
    [SerializeField]
    private ArgentSO _argent;
    [SerializeField]
    private SO_RoomType _stairRoom;

    [Space(10)]
    [Header("Color Btns")]
    [SerializeField]
    private Color _colorMoneyCan;
    [SerializeField]
    private Color _colorMoneyCant;

    [Space(10)]
    [Header("UI Btns")]
    public GameObject builderBtn;

    [Space(10)]
    [Header("UI Btn Prefabs")]
    public GameObject roomCategoryBtnPrefab;
    public GameObject categoryBtnPrefb;
    public GameObject StageBtnPrefab;

    [Space(10)]
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject roomConstructionPanel;
    public GameObject categoryPanel;
    public GameObject stagePanel;

    [Space(10)]
    [Header("Placement System Ref")]
    [SerializeField]
    private PlacementSystem _placementSystem;

    private bool _IsNextPanelCoroutineRunning = false;
    private bool _IsBackPanelCoroutineRunning = false;

    private void Awake()
    {
        //roomConstructionPanel.SetActive(false);
    }

    private void Start()
    {
        _placementSystem = FindObjectOfType<PlacementSystem>();

        //builderBtn.GetComponent<Button>().onClick.AddListener( OpenConstructionPanel );
        //builderBtn.GetComponent<Button>().onClick.AddListener( () => _placementSystem.ToggleGridVisualization() );
        PopulateCategories();
    }

    private void VerifyReferences()
    {
        if ( builderBtn == null || roomCategoryBtnPrefab == null || roomConstructionPanel == null || categoryPanel == null)
        {
            Debug.LogError("ConstructionUIManager: Missing references");
        }
    }

    public void OpenConstructionPanel()
    {
        //mainPanel.GetComponent<UIDissolveEffect>().DissolveOut();
        //roomConstructionPanel.GetComponent<UIDissolveEffect>().DissolveIn();

        StartCoroutine(NextPanel(mainPanel, roomConstructionPanel));

        _placementSystem.ToggleGridVisualization();
        //builderBtn.SetActive( false );
        //mainPanel.SetActive( false );
        //roomConstructionPanel.SetActive( true );
    }

    private void PopulateCategories()
    {
        foreach ( var category in data.rooms )
        {
            if ( category.RoomType == RoomType.BASE ) continue;
            if ( category.rooms.Any( room => room.isUnlocked == true ) == false ) continue;

            var button = Instantiate(categoryBtnPrefb, roomConstructionPanel.transform);
            button.name = category.RoomType.ToString();

            button.GetComponent<TextTraduction>().AssignID("runmenu_button" + category.RoomType.ToString());
            //button.GetComponent<ButtonManager>().buttonText = category.RoomType.ToString();
            button.GetComponent<ButtonManager>().UpdateUI();

            button.GetComponent<Button>().onClick.AddListener(() => OpenCategoryRooms( category ));

        }

        var stageButton = Instantiate(categoryBtnPrefb, roomConstructionPanel.transform);
        stageButton.GetComponent<TextTraduction>().AssignID("runmenu_buttonfloors");
        //stageButton.GetComponent<ButtonManager>().buttonText = "STAGES";
        stageButton.GetComponent<ButtonManager>().UpdateUI();

        stageButton.GetComponent<Button>().onClick.AddListener(() => OpenStagePanel());

        GenerateBackBtn(roomConstructionPanel, mainPanel).GetComponent<Button>().onClick.AddListener(() => _placementSystem.ToggleGridVisualization());
    }

    private void OpenCategoryRooms( RoomDB category )
    {
        //roomConstructionPanel.SetActive( false );
        //categoryPanel.SetActive( true );
        ClearCategoriesPanel();
        PopulateSubCategorie( category );

        StartCoroutine(NextPanel(roomConstructionPanel, categoryPanel));
    }

    private void PopulateSubCategorie( RoomDB category )
    {
        foreach ( var room in category.rooms )
        { 
            
            if( room.isUnlocked == false ) continue;

            var button = Instantiate( roomCategoryBtnPrefab, categoryPanel.transform );

            button.GetComponent<RoomBtnUI>().SetPrice(room.cost);

            //button.GetComponent<TextTraduction>().AssignID("roomname_" + room.roomName.ToString());
            button.GetComponent<RoomBtnUI>().SetName("roomname_" + room.roomName.ToString());

            button.GetComponent<RoomBtnUI>().button.onClick.AddListener( () => CreateNewRoom( room ) );
            button.name = room.roomName.ToString();
        }

        GenerateBackBtn(categoryPanel, roomConstructionPanel).GetComponent<Button>().onClick.AddListener(() => _placementSystem.CancelPlacement());
    }

    private void CreateNewRoom( SO_RoomType room )
    {
        _placementSystem.StartPlacement( room );
    }

    private void ClearCategoriesPanel()
    {
        foreach ( Transform child in categoryPanel.transform )
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearStagePanel()
    {
        foreach ( Transform child in stagePanel.transform )
        {
            Destroy(child.gameObject);
        }
    }

    private void OpenStagePanel()
    {
        ClearStagePanel();
        PopulateStagePanel();
        StartCoroutine(NextPanel(roomConstructionPanel, stagePanel));
    }

    private void PopulateStagePanel()
    {
        var addStageButton = Instantiate( roomCategoryBtnPrefab, stagePanel.transform );
        addStageButton.GetComponent<RoomBtnUI>().SetPrice(_stairRoom.cost);
        addStageButton.GetComponent<RoomBtnUI>().SetName("roomname_upperstair");

        addStageButton.GetComponent<RoomBtnUI>().button.onClick.AddListener(() => AddStage());

        var addBasementButton = Instantiate( roomCategoryBtnPrefab, stagePanel.transform );
        addBasementButton.GetComponent<RoomBtnUI>().SetPrice(_stairRoom.cost);
        addBasementButton.GetComponent<RoomBtnUI>().SetName("roomname_understair");

        addBasementButton.GetComponent<RoomBtnUI>().button.onClick.AddListener(() => AddBasement());

        GenerateBackBtn(stagePanel, roomConstructionPanel);
    }

    private void AddStage()
    {
        //Debug.Log("Add stage");
        _placementSystem.AddUpperStaire();
    }

    private void AddBasement()
    {
        //Debug.Log("Add basement");
        _placementSystem.AddUnderStaire();
    }
    
    private GameObject GenerateBackBtn(GameObject currentPanel, GameObject backPanel)
    {
        var backButton = Instantiate( categoryBtnPrefb, currentPanel.transform );
        backButton.GetComponent<ButtonManager>().buttonText = "BACK";
        backButton.GetComponent<ButtonManager>().UpdateUI();
        backButton.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(BackPanel(currentPanel, backPanel)));

        return backButton;
    }

    private IEnumerator NextPanel( GameObject currentPanel, GameObject nextPanel )
    {
        if ( _IsNextPanelCoroutineRunning || _IsBackPanelCoroutineRunning ) yield break;

        _IsNextPanelCoroutineRunning = true;
        currentPanel.GetComponent<UIDissolveEffect>().DissolveOut();

        nextPanel.SetActive(true);
        nextPanel.GetComponent<UIDissolveEffect>().DissolveIn();

        yield return new WaitForSeconds(0.5f);

        currentPanel.SetActive(false);
        _IsNextPanelCoroutineRunning = false;

    }

    private IEnumerator BackPanel( GameObject currentPanel, GameObject backPanel )
    {
        if ( _IsBackPanelCoroutineRunning || _IsNextPanelCoroutineRunning ) yield break;

        _IsBackPanelCoroutineRunning = true;
        currentPanel.GetComponent<UIDissolveEffect>().DissolveOut();

        backPanel.SetActive( true );
        backPanel.GetComponent<UIDissolveEffect>().DissolveIn();

        yield return new WaitForSeconds(0.5f);
        currentPanel.SetActive( false );
        _IsBackPanelCoroutineRunning = false;
    }
}
