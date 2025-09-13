using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LocalizationDataLanguageEditor
{
    public static List<string> langNames = new List<string>() { "English", "Spanish", "German", "French" };
    public static List<string> langCultureCode = new List<string>() { "en", "es", "de", "fr" };

    private static readonly Color backgroundColor = new Color(0.0f, 0.0f, 0f, 0.3f);
    private static int _langIndex = 0;

    public static void ShowLangs(LocalizationManager localizationManager)
    {
        EditorGUILayout.LabelField("Languages");


        Rect screenRect = GUILayoutUtility.GetRect(1, 1);
        Rect vertRect = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(screenRect.x - 13, screenRect.y - 1, screenRect.width + 17, vertRect.height + 9), backgroundColor);
        foreach (LocalizationDataLanguage localizationDataLanguage in localizationManager.languages)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(localizationDataLanguage.translationCode, GUILayout.Width(150));
            EditorGUILayout.LabelField(localizationDataLanguage.cultures[0], GUILayout.Width(30));
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                localizationManager.languages.Remove(localizationDataLanguage);
                // Delete the file
                string path = AssetDatabase.GetAssetOrScenePath(localizationDataLanguage);
                AssetDatabase.DeleteAsset(path);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(4);
        // show drop down of a list of langs to add
        EditorGUILayout.BeginHorizontal();
        _langIndex = EditorGUILayout.Popup(_langIndex, langNames.ToArray());

        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/Localization/Languages");
            string path = "Assets/Resources/Localization/Languages";
            string langAssetName = langNames[_langIndex];
            LocalizationDataLanguage localizationDataLanguage = ScriptableObjectUtility.CreateAsset<LocalizationDataLanguage>(langAssetName, path);
            Selection.activeObject = localizationManager;
            localizationDataLanguage.cultures.Add(langCultureCode[_langIndex]);
            localizationDataLanguage.translationCode = langNames[_langIndex];
            localizationManager.AddLanguage(localizationDataLanguage);
            
            EditorUtility.SetDirty(localizationManager);
            EditorUtility.SetDirty(localizationDataLanguage);
        }
        EditorGUILayout.EndHorizontal();
    }
}
