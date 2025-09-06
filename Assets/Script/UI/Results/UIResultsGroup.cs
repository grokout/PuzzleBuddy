using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResultsGroup : UIOptimizedListPrefab
{
    public Image imagePuzzle;
    public TextMeshProUGUI textLabel;
    public UIListController listResults;

    private Dictionary<int, PBPuzzle> _puzzles;
    void Start()
    {
        
    }

    public void Set(string puzzlename, Dictionary<int , PBPuzzle> puzzles)
    {
        _puzzles = puzzles;
        textLabel.text = puzzlename;
        DisplayList();
    }


    void DisplayList()
    {
        listResults.ClearAll();
        foreach (KeyValuePair<int, PBPuzzle> pair in _puzzles)
        {
            foreach (PBEntry entry in pair.Value.entries)
            {
                UIResultsRow uIResultsRow = listResults.CreateMarker<UIResultsRow>();
                uIResultsRow.Set(entry);
            }
        }
        listResults.ResizeContainer();

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = listResults.GetComponent<RectTransform>().sizeDelta.y + 60; 
        rectTransform.sizeDelta = size;
    }
}
