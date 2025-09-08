using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIFriendMarker : MonoBehaviour
{
    public Button buttonRemove;
    public TextMeshProUGUI textName;

    private FriendData _friendData;

    void Start()
    {
        
    }

    public void Set(FriendData friendData)
    {
        _friendData = friendData;
        textName.text = friendData.GetDisplayName();    
    }
}
