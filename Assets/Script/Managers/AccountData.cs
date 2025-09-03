using System;
using System.Collections.Generic;

public partial class AccountData
{
    public int userId = -1;
    public string spAlias = "";

    private string _displayName = "";

    public Dictionary<string,string> GetPlayerData()
    {
        return new System.Collections.Generic.Dictionary<string, string>
        {
            {"SPAlias", spAlias }
        };
    }

    public void AddTime(PBEntry entry)
    {
    }

    public string GetDisplayName()
    {
        if (string.IsNullOrEmpty(_displayName))
        {
           // return UserManager.instance.GetLoginName();
        }

        return _displayName;
    }

    public void SetDisplayName(string displayName)
    {
        _displayName = displayName;
    }
}
