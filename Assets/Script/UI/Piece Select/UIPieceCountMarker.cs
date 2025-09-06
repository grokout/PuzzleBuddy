using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPieceCountMarker : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public Button Button;

    private int _count;

    void Start()
    {
        Button.onClick.AddListener(() =>
        {
            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.CountChanged, new EventMsgManager.CountArgs(_count));
            UIManager.instance.HidePanel("UISelectPieceCount");
        });
    }

    public void Set(int count)
    {
        _count = count;
        textTitle.text = count.ToString();
    }
}
