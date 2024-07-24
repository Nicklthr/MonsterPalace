using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTraduction : MonoBehaviour
{

    public string textID;

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
            gameObject.GetComponent<TextMeshProUGUI>().text = LanguageHandler.Instance.GetTranslation(textID);
        }
    }

    public void AssignID(string newID)
    {
        textID = newID;
        GetText();
    }

}
