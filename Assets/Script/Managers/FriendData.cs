using System;

public class FriendData 
{
    public string id;
    public string displayName;

    public string GetDisplayName()
    {
        if (!string.IsNullOrEmpty(displayName))
        {
            return displayName;
        }

        return id;
    }
}
