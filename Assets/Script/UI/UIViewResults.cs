using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ZXing.OneD;

public class UIViewResults : UIBasePanel
{
    //public UIListController listResultGroups;
    public UIOptimizedList listResultGroups;
    void Start()
    {
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.SortFiltersChanged, OnSortFiltersChanged);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.LoadComplete, OnLoadComplete);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.UpdateEntriesForPuzzle, OnUpdateEntriesForPuzzle);

    }

    public override void Show(PanelData panelData = null)
    {
        base.Show();
        DisplayList();
    }


    void DisplayList()
    {
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
                        listResultGroups.Add(new OPListPBPuzzleData(pBPuzzle));
                    }
                }
            }
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
}

