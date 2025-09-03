using System.Collections.Generic;
using System.IO;

public class EventManager : Singleton<EventManager>
{
    public List<EventListData> eventListDatas = new List<EventListData>();
    //public List<PuzzleEvent> puzzleEvents = new List<PuzzleEvent>();

    // loads the json
    public void LoadEvents()
    {
        // TODO this would be a network call
      /*  string path = "res://TestData/Events.json";
        string text = Godot.FileAccess.GetFileAsString(path);
        JSONNode node = JSONObject.Parse(text);
        
        JSONNode jRoot = node[0];

        for (int i = 0; i < jRoot.Count; i++)
        {
            JSONNode jEvent = jRoot[i];
            EventListData eventListData = new EventListData();
            eventListData.Load(jEvent);
            eventListDatas.Add(eventListData);  
        }*/
    }
}

public class EventListData
{
   /* public string eventName;
    public string eventAssetName;

    public void Load(JSONNode node)
    {
        eventName = node["EventResource"]["Name"];
        eventAssetName = node["EventResource"]["AssetName"];
    }*/
}