using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPieceSelect : UIBasePanel
{
    public UIListController listCounts;
    public Button buttonBack;

    void Start()
    {
        buttonBack.onClick.AddListener(() => UIManager.instance.HidePanel("UISelectPieceCount"));
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show();

        DisplayLists();
    }


    void DisplayLists()
    {
        listCounts.ClearAll();


        List<int> counts = PBPuzzleManager.instance.GetCounts();


        // sort?

        foreach (int count in counts)
        {
            UIPieceCountMarker uIPieceCountMarker = listCounts.CreateMarker<UIPieceCountMarker>();
            uIPieceCountMarker.Set(count);
        }
        listCounts.ResizeContainer();
    }
}
