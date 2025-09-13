using Defective.JSON;
using System;
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

    [Flags]
    public enum VisualFlags
    {
        None = 0,
        Expanded = 1 << 1
    }


    private VisualFlags _visualFlags;

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
        jSONObject.SetField("_visualFlags", (int)_visualFlags);
        
        return jSONObject;
    }

    
    public void Load(JSONObject jPuzzle)
    {
        jPuzzle.GetField(ref brand, "brand");
        jPuzzle.GetField(ref name, "name");
        jPuzzle.GetField(ref pieceCount, "pieceCount");
        jPuzzle.GetField(ref upc, "upc");
        int vf = 0;
        jPuzzle.GetField(ref vf, "_visualFlags");
        _visualFlags = (VisualFlags)vf; 

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

    public bool ExpandedEntries()
    {
        return _visualFlags.HasFlag(VisualFlags.Expanded);
    }

    public void SetExpandedEntries(bool expanded)
    {
        if (expanded)
        {
            _visualFlags |= VisualFlags.Expanded;
        }
        else
        {
            _visualFlags &= ~VisualFlags.Expanded;
        }
    }

    public void ToggleExpand()
    {
        SetExpandedEntries(!ExpandedEntries());
    }


    public float GetFastestTime()
    {
        PBEntry pBEntry = GetFastestEntry();
        if (pBEntry != null)
        {
            return pBEntry.GetTime();
        }    

        return 1000000;
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


    public DateTime GetNewestData()
    {
        if (entries.Count == 0)
        {
            return DateTime.Now;
        }

        List<PBEntry> sorted = entries.OrderBy(o => o.date).ToList();
        return sorted[0].date;
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
