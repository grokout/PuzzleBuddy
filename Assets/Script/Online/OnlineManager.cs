using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class OnlineManager : Singleton<OnlineManager>
{
    private bool _connected = false;

    public AccountData myAccount = new AccountData();
    public OnlineFriends onlineFriends = new OnlineFriends();

    public void CreateService()
    {
        // Check perms
        //OS.RequestPermission("INTERNET");
        //OS.RequestPermission("ACCESS_NETWORK_STATE");
    }

    public void Connect()
    {
        _connected = true;
    }


    public bool IsConnected()
    {
        return _connected;
    }

    public void LoginUser(string username, string password)
    {
        SuprebaseOnline.instance.LoginUser(username, password);
    }

    public void RegisterNewUser(string username, string password)
    {
          SuprebaseOnline.instance.RegisterNewUser(username, password);
    }

    public string GetUserId()
    {
        return myAccount.userId.ToString();
        //return _service.GetUserId();
    }

    public int GetUserIdAsInt()
    {
        return myAccount.userId;
        //return _service.GetUserId();
    }

    public string GetDisplayName()
    {
        return myAccount.GetDisplayName();
    }

    public void SetDisplayName(int userId, string displayName)
    {
        if (userId == myAccount.userId)
        {
            myAccount.SetDisplayName(displayName);
        }
        else
        {
            onlineFriends.SetDisplayName(userId, displayName);
            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdatedFriendDisplayName, new EventMsgManager.UpdatedFriendDisplayNameArgs(userId));
        }
    }

    public void SetDisplayNameOnServer(int userId, string displayName)
    {
       // SuprebaseOnline.instance.SetDisplayName(displayName);

    }

    public void OnLogin()
    {
        //PBPuzzleManager.instance.FixDisplayNames();
        PBPuzzleManager.instance.LoadDB();
        onlineFriends.Load();

        SuprebaseOnline.instance.GetDisplayName(myAccount.userId);

        onlineFriends.OnLogin();

    }
}
