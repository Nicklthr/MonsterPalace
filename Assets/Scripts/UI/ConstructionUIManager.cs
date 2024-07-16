using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionUIManager : MonoBehaviour
{
    public SO_RoomsDB data;
    private ArgentSO _argent;

    [Space(10)]
    [Header("UI Btns")]
    public GameObject builderBtn;

    [Space(10)]
    [Header("UI Btn Prefabs")]
    public GameObject roomCategoryBtnPrefab;
    public GameObject categoryBtnPrefb;
    public GameObject StageBtnPrefab;

    [Space (10)]
    [Header("UI Panels")]
    public GameObject roomConstructionPanel;
    public GameObject categoryPanel;
    public GameObject stagePanel;

    private PlacementSystem _placementSystem;

    private void Awake()
    {
        roomConstructionPanel.SetActive(false);
        PopulateCategories();
    }

    private void Start()
    {
        _placementSystem = FindObjectOfType<PlacementSystem>();
        //VerifyReferences();
        builderBtn.GetComponent<Button>().onClick.AddListener( OpenConstructionPanel );
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
        builderBtn.SetActive( false );
        roomConstructionPanel.SetActive( true );
    }

    private void PopulateCategories()
    {
        foreach ( var category in data.rooms )
        {
            if ( category.RoomType == RoomType.BASE ) continue;

            var button = Instantiate(categoryBtnPrefb, roomConstructionPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = category.RoomType.ToString();
            button.GetComponent<Button>().onClick.AddListener(() => OpenCategoryRooms( category ));
        }

        var stageButton = Instantiate(categoryBtnPrefb, roomConstructionPanel.transform);
        stageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stage";
        stageButton.GetComponent<Button>().onClick.AddListener(() => OpenStagePanel());

        var backButton = Instantiate(categoryBtnPrefb, roomConstructionPanel.transform);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retour";
        backButton.GetComponent<Button>().onClick.AddListener(() => roomConstructionPanel.SetActive( false ) );
        backButton.GetComponent<Button>().onClick.AddListener(() => builderBtn.SetActive( true ) );
    }

    private void OpenCategoryRooms( RoomDB category )
    {
        roomConstructionPanel.SetActive( false );
        categoryPanel.SetActive( true );
        ClearCategoriesPanel();
        PopulateSubCategorie( category );
    }

    private void PopulateSubCategorie( RoomDB category )
    {
        foreach ( var room in category.rooms )
        {
            var button = Instantiate( roomCategoryBtnPrefab, categoryPanel.transform );
            button.GetComponent<RoomBtnUI>().SetPrice( room.cost );
            button.GetComponent<RoomBtnUI>().SetName( room.roomName.ToString() );

            button.GetComponent<Button>().onClick.AddListener( () => CreateNewRoom( room ) );
            button.name = room.roomName.ToString();
        }

        var backButton = Instantiate(categoryBtnPrefb, categoryPanel.transform);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retour";
        backButton.GetComponent<Button>().onClick.AddListener(() => categoryPanel.SetActive( false ));
        backButton.GetComponent<Button>().onClick.AddListener(() => roomConstructionPanel.SetActive( true ));
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
        roomConstructionPanel.SetActive( false );
        stagePanel.SetActive( true );
        ClearStagePanel();
        PopulateStagePanel();
    }

    private void PopulateStagePanel()
    {
        var addStageButton = Instantiate(StageBtnPrefab, stagePanel.transform);
        addStageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ajouter un étage";
        addStageButton.GetComponent<Button>().onClick.AddListener(() => AddStage());

        var addBasementButton = Instantiate(StageBtnPrefab, stagePanel.transform);
        addBasementButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ajouter un sous-sol";
        addBasementButton.GetComponent<Button>().onClick.AddListener(() => AddBasement());

        var backButton = Instantiate(categoryBtnPrefb, stagePanel.transform);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retour";
        backButton.GetComponent<Button>().onClick.AddListener(() => stagePanel.SetActive( false ));
        backButton.GetComponent<Button>().onClick.AddListener(() => roomConstructionPanel.SetActive( true ));

    }

    private void AddStage()
    {
        _placementSystem.AddStage(false);
    }

    private void AddBasement()
    {
        _placementSystem.AddStage(true);
    }
}
