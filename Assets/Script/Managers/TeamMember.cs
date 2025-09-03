using Defective.JSON;
using System;

public class TeamMember 
{
    public string name = "";
    public int id = -1;
    public string friendId = "";

    public JSONObject Serialize()
    {
        JSONObject jSon = new JSONObject();

        jSon.SetField("name", name);
        jSon.SetField("id", id);
        jSon.SetField("friendId", friendId);

        return jSon;
    }

    public void Deserialize(JSONObject jSon)
    {
        jSon.GetField(ref name, "name");
        jSon.GetField(ref id, "id");
        jSon.GetField(ref friendId , "friendId");
    }

    public void SetId(int id)
    {
        this.id = id; 
    }
}
