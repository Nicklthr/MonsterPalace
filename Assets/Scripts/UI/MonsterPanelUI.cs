using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPanelUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _monsterPanel;

    [SerializeField]
    private MonsterSelectionManager _monsterSelectionManager;


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
        _monsterPanel.SetActive( true );
    }

    private void HideMonsterPanel()
    {
        _monsterPanel.SetActive( false );
    }
}
