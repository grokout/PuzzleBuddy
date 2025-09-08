using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriends : UIBasePanel
{
    public UIListController listFriends;
    public Button buttonAdd;

    void Start()
    {
        
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show(panelData);
        listFriends.ClearAll();

        foreach (KeyValuePair<string,FriendData> friend in OnlineManager.instance.onlineFriends.friends)
        {
            UIFriendMarker friendMarker = listFriends.CreateMarker<UIFriendMarker>();
            friendMarker.Set(friend.Value);
        }

        listFriends.ResizeContainer();
    }
}
