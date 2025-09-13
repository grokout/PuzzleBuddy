using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZXing.OneD;

public class UIViewResults : UIBasePanel
{
    //public UIListController listResultGroups;
    public UIOptimizedList listResultGroups;
    public Button buttonFilter;
    public Button buttonSort;


    void Start()
    {
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.SortFiltersChanged, OnSortFiltersChanged);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.LoadComplete, OnLoadComplete);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.UpdateEntriesForPuzzle, OnUpdateEntriesForPuzzle);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.SortUpdated, OnSortUpdated);
        
        buttonSort.onClick.AddListener(() => UIManager.instance.ShowPanel("UIShowSorts"));
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show();
        DisplayList();
    }


    void DisplayList()
    {
        List<PBPuzzle> list = new List<PBPuzzle>(); 
        listResultGroups.ClearAll();
        Dictionary<string, Dictionary<string, Dictionary<int, PBPuzzle>>> puzzles = PBPuzzleManager.instance.puzzles;

        foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, PBPuzzle>>> brandPair in puzzles)
        {
            foreach (KeyValuePair<string, Dictionary<int, PBPuzzle>> namePair in brandPair.Value)
            {
                if (namePair.Value.Count > 0)
                {
                    KeyValuePair<int, PBPuzzle> f = namePair.Value.First();
                    PBPuzzle pBPuzzle = f.Value;
                    if (pBPuzzle.entries.Count > 0)
                    {
                        list.Add(pBPuzzle);
                    }
                }
            }
        }


        // now sort
        switch (PBPuzzleManager.instance.puzzleSortType)
        {
            case PuzzleSortType.BrandA:
                list = list.OrderBy(p => p.brand).ToList();
                break;
            case PuzzleSortType.BrandD:
                list = list.OrderByDescending(p => p.brand).ToList();
                break;
            case PuzzleSortType.PieceA:
                list = list.OrderBy(p => p.pieceCount).ToList();
                break;
            case PuzzleSortType.PieceD:
                list = list.OrderByDescending(p => p.pieceCount).ToList();
                break;
            case PuzzleSortType.TimeA:
                list = list.OrderBy(p => p.GetFastestTime()).ToList();
                break;
            case PuzzleSortType.TimeD:
                list = list.OrderByDescending(p => p.GetFastestTime()).ToList();
                break;
            case PuzzleSortType.EnterA:
                list = list.OrderBy(p => p.GetNewestData()).ToList();
                break;
            case PuzzleSortType.EnterD:
                list = list.OrderByDescending(p => p.GetNewestData()).ToList();
                break;
            case PuzzleSortType.NameA:
                list = list.OrderBy(p => p.name).ToList();
                break;
            case PuzzleSortType.NameD:
                list = list.OrderByDescending(p => p.name).ToList();
                break;
        }

        foreach (PBPuzzle pBPuzzle in list)
        {
            listResultGroups.Add(new OPListPBPuzzleData(pBPuzzle));
        }
    }

    void OnUpdateEntriesForPuzzle(EventMsgManager.GameEventArgs args)
    {
        DisplayList();
    }

    void OnLoadComplete(EventMsgManager.GameEventArgs args)
    {
        DisplayList();
    }

    void OnSortFiltersChanged(EventMsgManager.GameEventArgs args)
    {
        DisplayList();
    }

    void OnSortUpdated(EventMsgManager.GameEventArgs args)
    {
        DisplayList();
    }
}

