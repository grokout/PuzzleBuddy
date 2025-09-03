using Defective.JSON;
using System.Collections.Generic;
using System.Linq;


public class PBPuzzleCreationArgs
{
    public string brand;
    public string name;
    public int upc;
    public int pieceCount;
}

public class PBPuzzle
{
    public string brand;
    public string name;
    public int upc;
    public int pieceCount;
    public EventPlayerType eventPlayerType = EventPlayerType.Solo;
    public List<PBEntry> entries = new List<PBEntry> ();

    protected bool _allLoaded = false;

    public bool loadedFriendsData = false;

    public PBPuzzle()
    {
    }
    
    public PBPuzzle(PBEntry entry)
    {
        this.brand = entry.brand;
        this.name = entry.puzzleName;
        this.upc = entry.puzzleUpc;
        this.pieceCount = entry.puzzleCount;
        _allLoaded = true;
    }

    public virtual void AddTime(PBEntry entry)
    {
        //LoadAll();
        entry.pBPuzzle = this;
        entries.Add(entry);

        if (entry.userId == OnlineManager.instance.GetUserId())
        {
            Save();

            // update DB
            if (entry.dbId <= 0)
            {
                //SuprebaseOnline.instance.AddEntry(entry);
            }
        }
    }

    public void Remove(PBEntry entry)
    {
        entries.Remove(entry);
        Save();

        // Remove From Online
        if (entry.dbId > -1)
        {
            //SuprebaseOnline.instance.RemoveEntry(entry.dbId);
        }
        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.EntryRemoved);
    }

    public virtual string GetKey()
    {
        return brand + name;
    }


    public void Updated()
    {
        Save();
    }

    public virtual void Save()
    {
       /* _configFile = new ConfigFile();

        _configFile.SetValue("Entrys", "JSON", GetJson());
        _configFile.SetValue("Entrys", "brand", brand);
        _configFile.SetValue("Entrys", "name", name);
        _configFile.SetValue("Entrys", "pieceCount", pieceCount);

        _configFile.Save("user://Puzzle_" + GetKey() + ".cfg");*/
    }
    /*
    string GetJson()
    {
        JSONArray jArray = new JSONArray();

        for (int x = 0; x < entries.Count; x++)
        {
            if (entries[x].userId == OnlineManager.instance.GetUserId())
            {
                jArray.Add("Entry", entries[x].GetJson());
            }
        }

        return jArray.ToString();
    }*/
    /*
    public virtual void LoadAll()
    {
        if (_allLoaded)
        {
            return;
        }
        if (_configFile == null)
        {
            return;
        }

        string jStr = (string)_configFile.GetValue("Entrys", "JSON");

        JSONNode jRoot = JSONNode.Parse(jStr);

        if (jRoot != null && jRoot.Count > 0)
        {
            for (int i = 0; i < jRoot.Count; i++)
            {
                LoadEntry(jRoot[i]);
            }
        }
        brand = (string)_configFile.GetValue("Entrys", "brand");
        name = (string)_configFile.GetValue("Entrys", "name");
        pieceCount = (int)_configFile.GetValue("Entrys", "pieceCount");
        _allLoaded = true;

    }
    */
    protected virtual void LoadEntry(JSONObject jEntry)
    {
        PBEntry entry = new PBEntry();
        entry.Load(jEntry);
        entry.pBPuzzle = this;
        entries.Add(entry);
    }

    
    // loads only some parts
    public void PreLoad(string brand, string puzzleName)
    {
        /*this.brand = brand;
        this.name = puzzleName; 

        Error err = _configFile.Load("user://Puzzle_" + GetKey() + ".cfg");

        // If the file didn't load, ignore it.
        if (err != Error.Ok)
        {
            _configFile = null;
            return;
        }

        brand = (string)_configFile.GetValue("Entrys", "brand");
        name = (string)_configFile.GetValue("Entrys", "name");
        pieceCount = (int)_configFile.GetValue("Entrys", "pieceCount");


        //_configFile.Clear();
       // _configFile.Save("user://Puzzle_" + key + ".cfg");*/
    }
    /*
    public string GetInfo()
    {
        LoadAll();

        string info = "";

        PBEntry entry = GetFastestEntry();
        if (entry != null)
        {
            info += "My Fastest Time: " + entry.GetTimeString();
        }

        //info += "\nof " + entries.Count().ToString();
        return info;
    }

    public PBEntry GetFastestEntry()
    {
        if (entries.Count == 0)
        {
            return null;
        }
        List<PBEntry> sorted = entries.OrderBy(o => o.GetTime()).ToList();
        return sorted[0];
    }

    // ignore DNF
    public List<PBEntry> GetTimedEntries()
    {
        List<PBEntry> list = new List<PBEntry>();

        foreach (PBEntry pBEntry in entries)
        {
            if (!pBEntry.dnf)
            {
                list.Add(pBEntry);
            }
        }
        return list;
    }

    public virtual bool ShowFilters()
    {
        return true;
    }

    protected float GetMaxTime()
    {
        float maxTime = 0;

        foreach (PBEntry pBEntry in entries)
        {
            if (!pBEntry.dnf && pBEntry.GetTime() >maxTime)
            {
                maxTime = pBEntry.GetTime();
            }
        }

        // round up to the nearest 15 minutes. this is a guess
        int m = (int)(maxTime / 15);

        maxTime = (m + 1) * 15;
        return maxTime;
    }*/

}
