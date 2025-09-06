using Defective.JSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    }

    public void Remove(PBEntry entry)
    {
        entries.Remove(entry);
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




    public JSONObject Serialize()
    {
        JSONObject jSONObject = new JSONObject();

        JSONObject jArray = new JSONObject(JSONObject.Type.Array);
        jSONObject.SetField("entries", jArray);
        foreach (PBEntry entry in entries)
        {
            if (entry.userId == OnlineManager.instance.GetUserId())
            {
                jArray.Add(entry.Serialize());
            }
        }
        jSONObject.SetField("brand", brand);
        jSONObject.SetField("name", name);
        jSONObject.SetField("pieceCount", pieceCount);
        jSONObject.SetField("upc", upc);
        return jSONObject;
    }

    
    public void Load(JSONObject jPuzzle)
    {
        jPuzzle.GetField(ref brand, "brand");
        jPuzzle.GetField(ref name, "name");
        jPuzzle.GetField(ref pieceCount, "pieceCount");
        jPuzzle.GetField(ref upc, "upc");

        JSONObject jArray = jPuzzle.GetField("entries");
        if (jArray != null && jArray.list != null)
        {
            foreach (JSONObject jEntry in jArray.list)
            {
                PBEntry pBEntry = new PBEntry();
                pBEntry.Load(jEntry);

                AddTime(pBEntry);
            }
        }
        else
        {
            Debug.LogWarning("No entries for " + name);
        }


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
