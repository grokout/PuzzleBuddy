using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UserManager : Singleton<UserManager>
{
    private string _loginName = "";
    private string _password = "";
    private bool _autoLoginEnabled = false;

    public bool optionShowFriends = true;
    public SortType optionSortType = SortType.Time;

    public void Load()
    {

        _loginName = PlayerPrefs.GetString("_loginName");
        _password = PlayerPrefs.GetString("_password");

        _autoLoginEnabled = PlayerPrefs.GetInt("_autoLoginEnabled", 0) == 0 ? false : true;

        optionShowFriends = PlayerPrefs.GetInt("optionShowFriends", 0) == 0 ? false : true;
        optionSortType = (SortType)(int)PlayerPrefs.GetInt("optionSortType",0);

    }


    public void Save()
    {
        PlayerPrefs.SetString("_loginName", _loginName);
        PlayerPrefs.SetString("_password", _password);

        PlayerPrefs.SetInt("optionSortType", (int)optionSortType);
        PlayerPrefs.SetInt("optionShowFriends", optionShowFriends ? 1 : 0);
        PlayerPrefs.SetInt("_autoLoginEnabled", _autoLoginEnabled ? 1 : 0);
    }

    public string GetLoginName()
    {
        return _loginName; 
    }

    public string GetPassword()
    {
        return _password;
    }
    
    public void SetLoginCreds(string username, string password)
    {
        _loginName = username;
        _password = password;
        Save();
    }


    public void SetAutoLogin(bool autoLogin)
    {
        _autoLoginEnabled = autoLogin;
    }

    public bool CanAutoLogin()
    {
        return _autoLoginEnabled;
    }
}
