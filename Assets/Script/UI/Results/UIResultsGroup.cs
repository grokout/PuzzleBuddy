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
    public Button buttonExpand;

    private PBPuzzle _pBPuzzle;


    void Start()
    {
        
    }

    void Set(PBPuzzle pBPuzzle)
    {
        _pBPuzzle = pBPuzzle;
        textLabel.text = _pBPuzzle.name;
        DisplayList();
    }


    void DisplayList()
    {
        listResults.ClearAll();

            foreach (PBEntry entry in _pBPuzzle.entries)
            {
                UIResultsRow uIResultsRow = listResults.CreateMarker<UIResultsRow>();
                uIResultsRow.Set(entry);
            }
        
        listResults.ResizeContainer();

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = listResults.GetComponent<RectTransform>().sizeDelta.y + 60; 
        rectTransform.sizeDelta = size;
    }

    public override void SetData(OPListPBPuzzleData data)
    {
        base.SetData(data);
        OPListPBPuzzleData oPListPBPuzzleData = (OPListPBPuzzleData)data;
        Set(oPListPBPuzzleData.pBPuzzle);
    }
}

public class OPListPBPuzzleData : OptiizedListData
{
    public PBPuzzle pBPuzzle { get; set; }
    public UIOptimizedListPrefab marker = null;

    public OPListPBPuzzleData(PBPuzzle entry)
    {
        this.pBPuzzle = entry;
    }

    public override float GetHeight()
    {
        return 60 + (pBPuzzle.entries.Count * 40);
    }
}