using Defective.JSON;
using System;

public class FriendData 
{
    public string id;
    public string displayName = "";

    public string GetDisplayName()
    {
        if (!string.IsNullOrEmpty(displayName))
        {
            return displayName;
        }

        return id;
    }

    public JSONObject Serialize()
    {
        JSONObject json = new JSONObject();

        json.SetField("id", id);
        json.SetField("displayName",displayName);

        return json;
    }

    public void Load(JSONObject json)
    {
        json.GetField(ref id, "id");
        json.GetField(ref displayName, "displayName");
    }
}
