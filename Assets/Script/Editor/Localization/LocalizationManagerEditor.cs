using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{
    public enum Modes
    {
        SpreedSheets,
        Terms,
        Languages
    }

    const string assetName = "LocalizationManager";

    private Modes _mode = Modes.SpreedSheets;

    [MenuItem("Tools/OneBigBot/Localization")]
    public static void CreateAndOpenAsset()
    {
        // Check if the asset exists

        LocalizationManager localizationManager = Resources.Load<LocalizationManager>("LocalizationManager");

        if (localizationManager == null)
        {
            string path = "Assets/Resources";
            localizationManager = ScriptableObjectUtility.CreateAsset<LocalizationManager>(assetName, path);
        }

        Selection.activeObject = localizationManager;
        //
    }

    public override void OnInspectorGUI()
    {
        LocalizationManager localizationManager = (LocalizationManager)target;

        GUIStyle bigNameStyle = new GUIStyle();
        bigNameStyle.fontSize = 35;
        bigNameStyle.normal.textColor = Color.white;

        EditorGUILayout.LabelField("Localization", bigNameStyle);
        EditorGUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("SpreedSheets"))
        {
            _mode = Modes.SpreedSheets;
        }
        if (GUILayout.Button("Terms"))
        {
            _mode = Modes.Terms;
        }
        if (GUILayout.Button("Languages"))
        {
            _mode = Modes.Languages;
        }
        EditorGUILayout.EndHorizontal();

        switch (_mode)
        {
            case Modes.Languages:
                LocalizationDataLanguageEditor.ShowLangs(localizationManager);
                break;
            case Modes.Terms:
                LocalizationDataEntryEditor.ShowTerms(localizationManager);
                break;
            case Modes.SpreedSheets:
                ShowSpreedSheets(localizationManager);
                break;
        }
    }



    static void ShowSpreedSheets(LocalizationManager localizationManager)
    {
        EditorGUILayout.LabelField("Spreed Sheets");

        // add some tools

        /*if (GUILayout.Button("Import Lean Loc"))
        {
            ImportLeanLoc(localizationManager);
        }*/
        if (GUILayout.Button("Clear"))
        {
            Clear(localizationManager);
        }
        if (GUILayout.Button("Merge"))
        {
            Merge(localizationManager);
        }

        if (GUILayout.Button("Export"))
        {
            Export(localizationManager);
        }    

    }

    public static void SetAllDirty(LocalizationManager localizationManager)
    {
        EditorUtility.SetDirty(localizationManager);
        foreach (LocalizationDataLanguage localizationDataLanguage in localizationManager.languages)
        {
            EditorUtility.SetDirty(localizationDataLanguage);
        }
    }

    static void Clear(LocalizationManager localizationManager)
    {
        localizationManager.Clear();
        SetAllDirty(localizationManager);
    }


    static void Export(LocalizationManager localizationManager)
    {
        string path = Application.dataPath + "/LocalizationExport.csv";
        using StreamWriter file = new(path);

        // Header
        string header = "Key, Desc, ";
        foreach (LocalizationDataLanguage localizationDataLanguage in localizationManager.languages)
        {
            header += localizationDataLanguage.translationCode + ", ";
        }
        file.WriteLine(header);
        foreach (LocalizationDataTerm localizationDataTerm in localizationManager.GetAllTermsI())
        {
            string line = localizationDataTerm.key + ", , ";
            foreach (LocalizationDataLanguage localizationDataLanguage in localizationManager.languages)
            {
                line += localizationDataLanguage.GetValue(localizationDataTerm.key) + ", ";
            }
            file.WriteLine(line);
        }
    }

    static void Merge(LocalizationManager localizationManager)
    {
        string path = EditorUtility.OpenFilePanel("Merge CSV", "", "csv");
        if (path.Length != 0)
        {
            using StreamReader file = new(path);
            int i = 0;
            while (file.Peek() >= 0)
            {
                string line = file.ReadLine();
                Debug.Log(line);
                // skip header, TODO do we need to read this for lang order
                if (i != 0)
                {
                    string[] tokens = EditorUtils.SplitCSV(line);
                    string key = tokens[0];

                    int l = 0;
                    foreach (LocalizationDataLanguage localizationDataLanguage in localizationManager.languages)
                    {
                        if (l + 1 < tokens.Length)
                        {
                            string value = tokens[l + 1];
                            localizationManager.AddKey(key);
                            localizationDataLanguage.AddKey(key);
                            LocalizationDataEntry localizationDataEntry = localizationDataLanguage.terms[key];
                            localizationDataEntry.value = value;
                        }
                        else
                        {
                            break;
                        }
                        l++;
                    }
                }
                i++;
            }

            SetAllDirty(localizationManager);
        }        
    }
}
