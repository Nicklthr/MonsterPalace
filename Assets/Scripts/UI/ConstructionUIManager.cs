using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionUIManager : MonoBehaviour
{
    public SO_RoomsDB data;

    public GameObject builderButtonPrefab;
    public GameObject roomCategoryButtonPrefab;

    [Space (10)]
    public GameObject roomConstructionPanel;
    public GameObject categoryPanel;

    [Space(10)]
    public Transform roomButtonContainer;
    public Transform categoryButtonContainer;

    private void Start()
    {
        //VerifyReferences();
        roomConstructionPanel.SetActive(false);
        builderButtonPrefab.GetComponent<Button>().onClick.AddListener(OpenConstructionPanel);
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
        roomConstructionPanel.SetActive(true);
        PopulateCategories();
    }

    private void PopulateCategories()
    {
        foreach ( var category in data.rooms.Select(r => r.RoomType).Distinct() )
        {
            if (category == RoomType.BASE) continue;
            var button = Instantiate(roomCategoryButtonPrefab, categoryButtonContainer);
            button.GetComponentInChildren<Text>().text = category.ToString();
            button.GetComponent<Button>().onClick.AddListener(() => OpenCategoryRooms(category));
        }
    }

    private void OpenCategoryRooms(RoomType category)
    {
        
    }

    // Ajouter ici d'autres méthodes utiles pour la gestion UI
}
