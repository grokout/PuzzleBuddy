using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{

    public List<UIBasePanel> panels = new List<UIBasePanel>();

    void Start()
    {
        //SPPuzzlerManager.instance.Load();
        UserManager.instance.Load();
        //TeamMembersManager.instance.Load();
        PBPuzzleManager.instance.Load();


        foreach (UIBasePanel panel in panels)
        {
            if (panel != null)
            {
                if (panel.startOpen)
                {
                    panel.Show();
                }
                else
                {
                    panel.gameObject.SetActive(false);
                }
            }
        }

        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.LoadComplete);
    }

    public void ShowPanel(string panelName, PanelData panelData = null)
    {
        foreach (UIBasePanel panel in panels)
        {
            if (panel.name == panelName)
            {
                panel.Show(panelData);
            }
        }
    }

    public void HidePanel(string panelName)
    {
        foreach (UIBasePanel panel in panels)
        {
            if (panel.name == panelName)
            {
                panel.Hide();
            }
        }
    }


}


public class PanelData
{

}