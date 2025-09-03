using Defective.JSON;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public partial class OnlineFriends
{
    public Dictionary<string, FriendData> friends = new Dictionary<string, FriendData>();

    public void OnLogin()
    {
        //SuprebaseOnline.instance.GetFriends();
    }

    public string GetFriendDisplayName(string userId)
    {
        if (friends.ContainsKey(userId))
        {
            return friends[userId].GetDisplayName();
        }
        return "";
    }

    public List<FriendData> GetSortedFriendsList()
    {
        List<FriendData> list = new List<FriendData>();

        list = friends.Values.ToList();

        list = list.OrderBy(o => o.displayName).ToList();

        return list;
    }

    public void RemoveFriend(FriendData friendData)
    {
        //SuprebaseOnline.instance.RemoveFriend(friendData);
        friends.Remove(friendData.id);
        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.FriendsUpdated);
    }

    public void AddFriend(string friendId)
    {
        //SuprebaseOnline.instance.AddFriend(friendId);
        FriendData friendData = new FriendData()
        {
            id = friendId
        };
        friends.Add(friendId, friendData);
        int intId = 0;
        int.TryParse(friendId, out intId);
        //SuprebaseOnline.instance.GetDisplayName(intId);
        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.FriendsUpdated);
    }

    public void AddFriendReturn(string body)
    {
        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.FriendsUpdated);
    }

    public void OnGetFriends(string strFriends)
    {
        JSONObject json = new JSONObject(strFriends);

        foreach(JSONObject jFriend in json.list)
        {

            FriendData friendData = new FriendData()
            {
                id = jFriend.ToString().Replace("\"", "")
            };

            friends.Add(friendData.id, friendData);
            int intId = 0;
            int.TryParse(friendData.id, out intId);
            //SuprebaseOnline.instance.GetDisplayName(intId);
        }

        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.FriendsUpdated);
    }

    public void SetDisplayName(int userId, string displayName)
    {
        string strId = userId.ToString();
        if (!friends.ContainsKey(strId))
        {
            Debug.LogError("No friend with that id " + userId);
            return;
        }

        friends[strId].displayName = displayName;   
    }
}
