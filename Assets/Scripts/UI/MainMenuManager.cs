using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Michsky.UI.Dark.MainPanelManager;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private SaveManager _saveManager;

    public GameObject newGame;
    public List<PanelToHide> panelToHide = new List<PanelToHide>();

    public MainPanelManager mainPanelManager;
    [SerializeField] private Button _playButton;

    public bool debug = false;

    [System.Serializable]
    public class PanelToHide
    {
        public GameObject panel;
        public string panelName;
        public GameObject button;
    }

    void Start()
    {
        if(_playButton != null)
            _playButton.onClick.AddListener( () => GameManager.Instance.StartNewRun() );

        if (debug)
            return;

        _saveManager = FindObjectOfType<SaveManager>();

        if ( _saveManager.checkSaveExist() )
        {
            newGame.SetActive(false);

            foreach (PanelToHide panel in panelToHide )
            {
                panel.panel.SetActive(true);

                if (panel.button != null)
                {
                    panel.button.SetActive(true);
                }

                foreach (PanelItem item in mainPanelManager.panels)
                {
                    if (item.panelName == panel.panelName)
                    {
                        item.isLock = false;
                    }
                }
            }
        }
        else
        {
            newGame.SetActive( true );

            foreach (PanelToHide panel in panelToHide)
            {
                panel.panel.SetActive(false);

                if (panel.button != null)
                {
                    panel.button.SetActive(false);
                }

                foreach (PanelItem item in mainPanelManager.panels)
                {
                    if (item.panelName == panel.panelName)
                    {
                        item.isLock = true;
                    }
                }
            }
        }
        
    }

}
