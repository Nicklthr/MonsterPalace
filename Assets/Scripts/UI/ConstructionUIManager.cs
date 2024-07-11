using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ConstructionUIManager : MonoBehaviour
{
    public GameObject roomButtonPrefab;
    public GameObject builderButtonPrefab;
    public GameObject roomCategoryButtonPrefab;

    public GameObject roomConstructionPanel;

    private void Start()
    {
      // Check if the references are set
      if( roomButtonPrefab == null || builderButtonPrefab == null || roomCategoryButtonPrefab == null || roomConstructionPanel == null )
      {
        Debug.LogError( "ConstructionUIManager: Missing references" );
        return;
      }
      
      // Make sure the panel is not active
      roomConstructionPanel.SetActive( false );
    }
}