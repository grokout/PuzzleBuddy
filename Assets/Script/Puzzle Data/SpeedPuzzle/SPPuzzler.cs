using Defective.JSON;

public class SPPuzzler
{
    public string name;


    public JSONObject Serialize()
    {
        JSONObject jSon = new JSONObject();

        jSon.SetField("name", name);

        return jSon;
    }

    public void Deserialize(JSONObject jSon)
    {
        jSon.GetField(ref name, "name");
    }

}

