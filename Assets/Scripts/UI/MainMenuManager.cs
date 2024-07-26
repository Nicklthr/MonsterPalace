using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private SaveManager _saveManager;

    public GameObject newGame;
    public GameObject[] panelToHide;

    public bool debug = false;

    void Start()
    {
        if (debug)
            return;

        _saveManager = FindObjectOfType<SaveManager>();

        if ( _saveManager.checkSaveExist() )
        {
            newGame.SetActive(false);

            foreach ( GameObject panel in panelToHide )
            {
                panel.SetActive(true);
            }
        }
        else
        {
            newGame.SetActive(true);

            foreach (GameObject panel in panelToHide)
            {
                panel.SetActive(false);
            }
        }
        
    }

}
