using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewPuzzle : UIBasePanel
{
    public Image imagePuzzle;
    public TextMeshProUGUI textLabel;
    public TextMeshProUGUI textLabel2;
    public UIListController listResults;
    public RectTransform rectTransformPuzzle;
    public Button buttonBack;

    private PBPuzzle _pbPuzzle;


    void Start()
    {
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.UpdateEntriesForPuzzle, OnUpdateEntriesForPuzzle);
        buttonBack.onClick.AddListener(() => UIManager.instance.HidePanel("UIViewPuzzle"));

        if (_pbPuzzle != null )
        {
            DisplayList();
        }
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show(panelData);
        UIViewPuzzleData uIViewPuzzleData = (UIViewPuzzleData)panelData;

        _pbPuzzle = uIViewPuzzleData.bPuzzle;
        DisplayList();

        // check for any friends update
        PBPuzzleManager.instance.GetFriendEntriesFor(_pbPuzzle);

        textLabel.text = _pbPuzzle.name;
        textLabel2.text = _pbPuzzle.pieceCount.ToString() + " - " + _pbPuzzle.brand;
    }

    void DisplayList()
    {
        listResults.ClearAll();

        // Sort. let sort by date newest first for now
        List<PBEntry> sortedList = new List<PBEntry>(_pbPuzzle.entries);
        sortedList = sortedList.OrderBy(p => p.GetTime()).ToList();

        foreach (PBEntry entry in sortedList)
        {
            UIResultsRow uIResultsRow = listResults.CreateMarker<UIResultsRow>();
            uIResultsRow.Set(entry);
        }

        listResults.ResizeContainer();

        Vector2 size = rectTransformPuzzle.sizeDelta;
        size.y = 120 + (sortedList.Count * 60);
        rectTransformPuzzle.sizeDelta = size;
    }

    void OnUpdateEntriesForPuzzle(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.PuzzleArgs puzzleArgs = (EventMsgManager.PuzzleArgs)args;   
        if (puzzleArgs.pBPuzzle == _pbPuzzle)
        {
            DisplayList();
        }        

    }
}


public class UIViewPuzzleData : PanelData
{

    public PBPuzzle bPuzzle;

    public UIViewPuzzleData(PBPuzzle bPuzzle)
    {
        this.bPuzzle = bPuzzle;
    }
}