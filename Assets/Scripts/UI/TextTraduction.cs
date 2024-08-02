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
    public bool customUIText = false;


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
            else if (customUIText)
            {
                ModalWindowManager uiObject = gameObject.GetComponent<ModalWindowManager>();
                uiObject.title = LanguageHandler.Instance.GetTranslation(textID+"_title");
                uiObject.description = LanguageHandler.Instance.GetTranslation(textID+"_desc");
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
