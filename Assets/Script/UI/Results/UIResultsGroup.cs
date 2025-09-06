using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIResultsGroup : UIOptimizedListPrefab
{
    public Image imagePuzzle;
    public TextMeshProUGUI textLabel;
    public UIListController listResults;
    public Button buttonExpand;

    private PBPuzzle _pBPuzzle;


    void Start()
    {
        buttonExpand.onClick.AddListener(ToggleExpand);
        
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

        buttonExpand.gameObject.SetActive(_pBPuzzle.entries.Count > 1);


        buttonExpand.transform.rotation = _pBPuzzle.ExpandedEntries() ? Quaternion.identity * Quaternion.Euler(0, 0, 90) :  Quaternion.identity * Quaternion.Euler(0,0,180);

        // Sort. let sort by date newest first for now
        List<PBEntry> sortedList = new List<PBEntry>(_pBPuzzle.entries);
        sortedList = sortedList.OrderBy(p => p.GetTime()).ToList();

       
        
        foreach (PBEntry entry in sortedList)
        {
            UIResultsRow uIResultsRow = listResults.CreateMarker<UIResultsRow>();
            uIResultsRow.Set(entry);

            if (!_pBPuzzle.ExpandedEntries())
            {
                break;
            }
        }
        

        
        listResults.ResizeContainer();

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = _data.GetHeight();
        rectTransform.sizeDelta = size;
    }

    void ToggleExpand()
    {
        _pBPuzzle.ToggleExpand();

        DisplayList();

        uIOptimizedList.ResizeContainer();  
        uIOptimizedList.UpdateVisualList();
        PBPuzzleManager.instance.Save();

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
        int c = pBPuzzle.entries.Count;
        if (c > 1 && !pBPuzzle.ExpandedEntries())
        {
            c = 1;
        }
        return 60 + (c * 40);
    }
}