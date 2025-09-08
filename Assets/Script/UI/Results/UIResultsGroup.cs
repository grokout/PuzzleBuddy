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
    public Button buttonInfo;
    public bool alwaysExpanded = false;

    private PBPuzzle _pBPuzzle;


    void Start()
    {
        buttonExpand.onClick.AddListener(ToggleExpand);
        if (buttonInfo != null)
        {
            buttonInfo.onClick.AddListener(() =>
            {
                UIManager.instance.ShowPanel("UIViewPuzzle", new UIViewPuzzleData(_pBPuzzle));  
            });
        }

        if (alwaysExpanded && buttonExpand != null)
        {
            buttonExpand.gameObject.SetActive(false);
        }
        
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
            if (entry.userId != OnlineManager.instance.GetUserId())
            {
                continue;
            }
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

        if (uIOptimizedList != null)
        {
            uIOptimizedList.ResizeContainer();
            uIOptimizedList.UpdateVisualList();
        }
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
    public bool alwaysExpanded = false;
    public float baseHeight = 60;
    public float entryHeight = 40;

    public OPListPBPuzzleData(PBPuzzle entry)
    {
        this.pBPuzzle = entry;
    }

    public override float GetHeight()
    {
        int c = 0;
        foreach (PBEntry entry in pBPuzzle.entries)
        {
            if (entry.userId == OnlineManager.instance.GetUserId())
            {
                c++;
            }
        }
        
        if (!alwaysExpanded)
        {
            if (c > 1 && !pBPuzzle.ExpandedEntries())
            {
                c = 1;
            }
        }
        return baseHeight + (c * entryHeight);
    }
}