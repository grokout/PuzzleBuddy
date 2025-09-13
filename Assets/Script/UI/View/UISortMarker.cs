using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISortMarker : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public Toggle toggle;

    private PuzzleSortType _puzzleSortType;
    void Start()
    {
        toggle.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                UIManager.instance.HidePanel("UIShowSorts");
                PBPuzzleManager.instance.puzzleSortType = _puzzleSortType;
                PBPuzzleManager.instance.Save();
                EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.SortUpdated);
            }
        });
    }

    public void Set(PuzzleSortType puzzleSortType)
    {
        _puzzleSortType = puzzleSortType;
        textLabel.text = LocalizationManager.GetTranslation("PuzzleSortType."+_puzzleSortType.ToString());
        if (PBPuzzleManager.instance.puzzleSortType == _puzzleSortType)
        {
            toggle.isOn = true;
        }
    }


}
