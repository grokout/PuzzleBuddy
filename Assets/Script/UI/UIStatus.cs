using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStatus : UIBasePanel
{
    public TextMeshProUGUI textMessage;

    void Start()
    {
        
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show();

        if (panelData != null)
        {
            UIStatusData uIStatusData = (UIStatusData)panelData;

            textMessage.text = uIStatusData.msg;
        }
    }
}

public class UIStatusData : PanelData
{
    public string msg;

    public UIStatusData(string msg)
    {
        this.msg = msg;
    }
}
