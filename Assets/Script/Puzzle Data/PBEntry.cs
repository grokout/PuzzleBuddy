using Defective.JSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PBEntry
{
    public PBPuzzle pBPuzzle;

    public DateTime date;
    
    public string userId;
    public bool dnf = false;
    public int placed = 0; // used if dnf
    public string brand;
    public string puzzleName;
    public int puzzleCount;
    public int puzzleUpc = 0;
    public int tempRank = 0;

    private float _time;
    private List<string> _teamMembers = new List<string>();
    public int dbId = -1;

    public JSONObject Serialize()
    {
        JSONObject j = new JSONObject();

        j.SetField("date", date.ToString());
        j.SetField("time", _time);
        j.SetField("userId", userId);
        j.SetField("dnf", dnf);
        j.SetField("placed", placed);
        j.SetField("dbId", dbId);

        JSONObject jArray = new JSONObject(JSONObject.Type.Array);

        foreach (string teamMember in _teamMembers)
        {
            jArray.Add(teamMember);
        }
        j.SetField("TeamMembers", jArray);

        j.SetField("brand", brand);
        j.SetField("puzzleName", puzzleName);
        j.SetField("puzzleCount", puzzleCount);
        j.SetField("puzzleUpc", puzzleUpc);

        return j;
    }

    public virtual void Load(JSONObject json)
    {
        string dateStr = "";
        json.GetField(ref dateStr, "date");
        if (!string.IsNullOrEmpty(dateStr))
        {
            date = DateTime.Parse(dateStr);
        }
        json.GetField(ref _time, "time");

        json.GetField(ref userId, "userId");
        json.GetField(ref placed, "placed");

        if (json.HasField("dbId"))
        {
            json.GetField(ref dbId, "dbId");
        }
        Debug.Log("Load dbid " + dbId);
        if (json.HasField("dnf"))
        {
            json.GetField(ref dnf, "dnf");
        }
        else
        {
            dnf = false;
        }

        JSONObject jArray = json.GetField("TeamMembers");
        if (jArray != null && jArray.list != null)
        {
            foreach(JSONObject jObject in jArray.list)
            {
                _teamMembers.Add(jObject.stringValue);
            }
        }

        json.GetField(ref brand, "brand");
        json.GetField(ref puzzleName, "puzzleName");
        json.GetField(ref puzzleCount, "puzzleCount");
        json.GetField(ref puzzleUpc, "puzzleUpc");
    }

    public void LoadFromBD(JSONObject json)
    {
        json.GetField(ref dbId, "id");
        json.GetField(ref userId, "user_id");
        json.GetField(ref dnf, "dnf");
        json.GetField(ref placed, "placed");
        json.GetField(ref _time,"time");
        json.GetField(ref brand, "brand");
        json.GetField(ref puzzleName, "puzzleName");
        json.GetField(ref puzzleCount, "puzzleCount");
        json.GetField(ref puzzleUpc, "puzzleUpc");
    }

    public void LoadFromBD(List<string> cols)
    {
        int d;
        int.TryParse(cols[0], out d);
        dbId = d;
        userId = cols[2];
        dnf = cols[3] != "0";
        int.TryParse(cols[4], out placed);
        date = DateTime.Parse(cols[6]);
        float.TryParse(cols[5], out _time);
        string team = cols[7];

        brand = cols[8];
        puzzleName = cols[9];
        int.TryParse(cols[10], out puzzleCount);
        int.TryParse(cols[14], out puzzleUpc);
    }

    public virtual string GetInfoText()
    {

        string info = date.ToString("MMMM dd, yyyy");
        /*foreach (int teamMemberId in _teamMembers)
        {
            info += "\n" + TeamMembersManager.instance.GetTeamMember(teamMemberId).name;
        }*/
        return info;
    }


    public string GetInfoText2()
    {
        return OnlineManager.instance.onlineFriends.GetFriendDisplayName(userId);
    }

    public float GetTime()
    {
        if (dnf)
        {
            return _time == 0 ? 1000000 : _time;
        }
        return _time;
    }

    public void SetTime(float time)
    {
        _time = time;
    }

    public string GetTimeString()
    {
        if (dnf || _time == 0)
        {
            if (placed > 0)
            {
                return placed.ToString() + "\nplaced";
            }
            else
            {
                return "DNF";
            }
        }

        int hour = (int)(_time / 60);
        int minute = (int)(_time - (60 * hour));
        int sec = Mathf.RoundToInt((_time % 1) * 60);
      
        string tStr = hour.ToString() + ":" + minute.ToString() + ":" + sec.ToString();

        return tStr;
    }

    public string GetTimeStringInMinutes()
    {
        if (dnf)
        {
            return "DNF";
        }

        string tStr = _time.ToString("n2");

        return tStr;
    }

    public bool Finished()
    {
        return !dnf;
    }

    public virtual bool CanEdit()
    {
        return userId == OnlineManager.instance.GetUserId();
    }

    public virtual int GetRows()
    {
        return 1 + _teamMembers.Count;
    }

    public float GetPScore()
    {
        int pieceCount = puzzleCount;

        float pScore = 0;

        if (dnf && placed == 0)
        {
            return 0;
        }

        if (placed != 0)
        {
            pieceCount = placed;
        }
        if (pieceCount > 0 && GetTime() > 0)
        {
            pScore = pieceCount / GetTime();
        }


        return pScore;
    }

    public string GetTeamMemberName(int index)
    {
        if (_teamMembers.Count > index)
        {
           return _teamMembers[index];
           // return TeamMembersManager.instance.GetTeamMember(_teamMembers[index]).name;
        }

        return "";
    }

    public bool HasTeam()
    {
        return _teamMembers.Count > 0;
    }

    public void SetTeamMembers(List<string> teamMembers)
    {
        _teamMembers = teamMembers;
    }

    public void SetDBId(int dbId)
    {
        this.dbId = dbId;
    }

    public void Updated()
    {
        /*pBPuzzle.Save();

        if (dbId != 0)
        {
            SuprebaseOnline.instance.UpdateEntry(this);
        }*/
    }
}
