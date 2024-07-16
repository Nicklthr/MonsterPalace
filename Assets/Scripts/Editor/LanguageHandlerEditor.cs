using Esper.ESave;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageHandler))]
public class LanguageHandlerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LanguageHandler languageHandler = (LanguageHandler)target;

        if (GUILayout.Button("English"))
        {
            LanguageHandler.Instance.ChangeToEnglish();
        }

        if (GUILayout.Button("French"))
        {
            LanguageHandler.Instance.ChangeToFrench();
        }



    }
}