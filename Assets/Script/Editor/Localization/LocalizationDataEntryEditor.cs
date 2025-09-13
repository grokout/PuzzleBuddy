using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Web;
using System.Collections;
using System.Linq;

public static class LocalizationDataEntryEditor 
{
    private static readonly Color backgroundColor = new Color(0.0f, 0.0f, 0f, 0.3f);
    private static readonly Color termColor = new Color(0.0f, 0.0f, 0.2f, 0.3f);
    private static string _addKey = "";
    private static LocalizationDataTerm _selectedLocalizationDataTerm;

    public static void ShowTerms(LocalizationManager localizationManager)
    {
        EditorGUILayout.LabelField("Terms");


        // TODO add a search field


        // list
        Rect screenRect = GUILayoutUtility.GetRect(1, 1);
        Rect vertRect = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 17, vertRect.height + 9), backgroundColor);

        List<LocalizationDataTerm> sortedList = new(localizationManager.GetAllTermsI());
        sortedList = sortedList.OrderBy(o => o.key).ToList();
        foreach (LocalizationDataTerm localizationDataTerm in sortedList)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                localizationManager.RemoveKey(localizationDataTerm);
                LocalizationManagerEditor.SetAllDirty(localizationManager);
                break;
            }
            GUI.backgroundColor = _selectedLocalizationDataTerm == localizationDataTerm ? Color.green : Color.white;
            if (GUILayout.Button(localizationDataTerm.key, GUILayout.Width(250)))
            {
                _selectedLocalizationDataTerm = localizationDataTerm != _selectedLocalizationDataTerm ? localizationDataTerm : null ;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.LabelField(localizationDataTerm.GetKeyCat());
            EditorGUILayout.EndHorizontal();

            if (_selectedLocalizationDataTerm == localizationDataTerm)
            {
                ShowTerm(localizationManager, _selectedLocalizationDataTerm);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);

        ShowAddField(localizationManager);
    }

    static void ShowAddField(LocalizationManager localizationManager)
    {
        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("New Key", GUILayout.Width(100));
        _addKey = EditorGUILayout.TextField(_addKey);
        if (GUILayout.Button("Add", GUILayout.Width(50)))
        {
            // see if we can add new key
            localizationManager.AddKey(_addKey);
            LocalizationManagerEditor.SetAllDirty(localizationManager);
        }
        EditorGUILayout.EndHorizontal();
    }

    
    static void ShowTerm(LocalizationManager localizationManager, LocalizationDataTerm localizationDataTerm)
    {
        Rect screenRect = GUILayoutUtility.GetRect(1, 1);
        Rect vertRect = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 17, vertRect.height + 9), termColor);

        foreach (LocalizationDataLanguage localizationDataLanguage in localizationManager.languages)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(localizationDataLanguage.translationCode, GUILayout.Width(80));
            LocalizationDataEntry localizationDataEntry = localizationDataLanguage.terms[localizationDataTerm.key];
            localizationDataEntry.value = EditorGUILayout.TextField(localizationDataEntry.value);
            if (GUILayout.Button("Auto Trans", GUILayout.Width(80)))
            {
                string trans = "";
                if (TryAutoTranslate(localizationManager.languages[0].cultures[0], localizationDataLanguage.cultures[0], localizationManager.languages[0].terms[localizationDataTerm.key].value, ref trans))
                {
                    localizationDataEntry.value = trans;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorUtility.SetDirty(localizationDataLanguage);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
    }

    private static bool TryAutoTranslate(string languageCodeInput, string languageCodeOutput, string wordInput, ref string wordOutput)
    {
        return false;
        /*
        try
        {
            var url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", languageCodeInput, languageCodeOutput, HttpUtility.UrlEncode(wordInput));
            var webClient = new System.Net.WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = webClient.DownloadString(url);

            wordOutput = result.Substring(4, result.IndexOf("\",\"", 4, System.StringComparison.Ordinal) - 4);

            return true;
        }
        catch
        {
            return false;
        }*/
    }
}
