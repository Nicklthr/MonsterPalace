using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionUIManager : MonoBehaviour
{
    public SO_RoomsDB data;

    [Space(10)]
    [Header("UI Btn Prefabs")]
    public GameObject builderButtonPrefab;
    public GameObject roomCategoryButtonPrefab;

    [Space (10)]
    [Header("UI Panels")]
    public GameObject roomConstructionPanel;
    public GameObject categoryPanel;

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
        builderButtonPrefab.GetComponent<Button>().onClick.AddListener( OpenConstructionPanel );
    }

    private void VerifyReferences()
    {
        if ( builderButtonPrefab == null || roomCategoryButtonPrefab == null || roomConstructionPanel == null || categoryPanel == null)
        {
            Debug.LogError("ConstructionUIManager: Missing references");
        }
    }


    public void OpenConstructionPanel()
    {
        builderButtonPrefab.SetActive( false );
        roomConstructionPanel.SetActive( true );
    }

    private void PopulateCategories()
    {
        foreach ( var category in data.rooms )
        {
            if ( category.RoomType == RoomType.BASE ) continue;

            var button = Instantiate(roomCategoryButtonPrefab, roomConstructionPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = category.RoomType.ToString();
            button.GetComponent<Button>().onClick.AddListener(() => OpenCategoryRooms( category ));
        }

        var backButton = Instantiate(roomCategoryButtonPrefab, roomConstructionPanel.transform);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retour";
        backButton.GetComponent<Button>().onClick.AddListener(() => roomConstructionPanel.SetActive( false ) );
        backButton.GetComponent<Button>().onClick.AddListener(() => builderButtonPrefab.SetActive( true ) );
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
            var button = Instantiate(roomCategoryButtonPrefab, categoryPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = room.roomName.ToString();
            button.GetComponent<Button>().onClick.AddListener( () => CreateNewRoom( room ) );
        }

        var backButton = Instantiate(roomCategoryButtonPrefab, categoryPanel.transform);
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
}
