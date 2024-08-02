using Michsky.UI.Dark;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTraduction : MonoBehaviour
{

    public string textID;
    public bool customUIButton = false;

    // Start is called before the first frame update
    void Start()
    {

        LanguageHandler.Instance.LanguageHandlerSubscription(this);

        GetText();

    }


    public void GetText()
    {
        if (textID != null && textID != "")
        {  

            if (customUIButton)
            {
                ButtonManager uiObject = gameObject.GetComponent<ButtonManager>();
                uiObject.buttonText = LanguageHandler.Instance.GetTranslation(textID);
            }
            else
            {
                gameObject.GetComponent<TextMeshProUGUI>().text = LanguageHandler.Instance.GetTranslation(textID);
            }
        }
    }

    public void AssignID(string newID)
    {
        textID = newID;
        GetText();
    }

    public void OnDestroy()
    {
        LanguageHandler.Instance.LanguageHandlerRemoval(this);
    }

}
