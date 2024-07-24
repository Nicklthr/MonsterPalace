using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageHandler : MonoBehaviour
{
    [SerializeField] private TextAsset fullTranslation;
    private LanguageType curSelectedLanguage = LanguageType.English;
    private const string LANGUAGE_KEY = "CurLanguageKey";

    public static LanguageHandler Instance;
    private Dictionary<string, string> translations = new Dictionary<string, string>();

    public List<TextTraduction> txtList;


    public void LanguageHandlerSubscription(TextTraduction txt)
    {
        txtList.Add(txt);
    }

    public void LanguageHandlerRemoval(TextTraduction txt)
    {
        txtList.Remove(txt);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        ReadTextFile();
        ReloadLanguage();
    }

    private void ReadTextFile()
    {
        var document = fullTranslation.text.Split('\t', '\n');
        var rowSize = fullTranslation.text.Split('\n')[0].Split('\t').Length;
        var columnSize = document.Length / rowSize;

        for (int i = 1; i < rowSize; i++)
        {
            for (int j = 0; j < columnSize; j++)
            {
                var content = document[i + rowSize * j];
                var stringID = document[j * rowSize] + i;
                translations.Add(stringID.ToLower(), content);
            }
        }
    }

    public string GetTranslation(string stringID)
    {
        var fullStringID = stringID + (int)curSelectedLanguage;
        return translations[fullStringID.ToLower()];
    }

    private void ReloadLanguage()
    {
        if (PlayerPrefs.HasKey(LANGUAGE_KEY))
        {
            curSelectedLanguage = (LanguageType)PlayerPrefs.GetInt(LANGUAGE_KEY);
        }
        else
        {
            curSelectedLanguage = LanguageType.English;
        }

        foreach(TextTraduction t in txtList)
        {
            t.GetText();
        }

    }


    private void SelectNewLanguage(LanguageType languageType)
    {
        curSelectedLanguage = languageType;
        PlayerPrefs.SetInt(LANGUAGE_KEY, (int)languageType);
        ReloadLanguage();
    }


    public void ChangeToFrench()
    {
        SelectNewLanguage(LanguageType.French);
    }

    public void ChangeToEnglish()
    {
        SelectNewLanguage(LanguageType.English);
    }

}

public enum LanguageType
{
    English = 1,
    French = 2,
}
