using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowSorts : UIBasePanel
{
    public UIListController listSortTypes;
    public Button buttonCancel;

    void Start()
    {
        BuildSortLists();
        buttonCancel.onClick.AddListener(() => UIManager.instance.HidePanel("UIShowSorts"));
    }


    void BuildSortLists()
    {
        foreach (PuzzleSortType puzzleSortType in Enum.GetValues(typeof(PuzzleSortType)))
        {
            UISortMarker uISortMarker = listSortTypes.CreateMarker<UISortMarker>();
            uISortMarker.Set(puzzleSortType);
        }
    }

}
