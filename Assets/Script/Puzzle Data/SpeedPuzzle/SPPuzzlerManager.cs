using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class SPPuzzlerManager : Singleton<SPPuzzlerManager>
{
    /*const string SPPrefix = "https://www.speedpuzzling.com";
    public const string resultsPage = "https://www.speedpuzzling.com/results.html";

    //private Dictionary<string,SPPuzzler> puzzlers = new Dictionary<string, SPPuzzler>();

    private Dictionary<string, SPPuzzleEvent> _events = new Dictionary<string, SPPuzzleEvent>();

    // Is this the best way
    private Action<SPPuzzleEvent> _callBack;
    private Action _callBackEventsUpdated;
    private string _resultPuzzleName;

    public void GetSPData(string spPuzzleDataName, HttpRequest request, Action<SPPuzzleEvent> callBack)
    {
        _resultPuzzleName = spPuzzleDataName;
        _callBack = callBack;
        // Check if we have a local cached copy
        if (HasCachedData(spPuzzleDataName))
        {
            if (_events[spPuzzleDataName] == null)
            {
                SPPuzzleEvent puzzleEvent = new SPPuzzleEvent();
                puzzleEvent.keyName = spPuzzleDataName;
                puzzleEvent.LoadAll();
                _events[spPuzzleDataName] = puzzleEvent;
            }
            callBack(_events[spPuzzleDataName]);
            return;
        }

        // if not grab and cache it
        request.Request(spPuzzleDataName);
    }

    bool HasCachedData(string spPuzzleDataName)
    {
        //return false;
        if (!_events.ContainsKey(spPuzzleDataName))
        { 
            return false; 
        }
        // TODO there has to be a better way for this
        ConfigFile configFile = new ConfigFile();   
        Error err = configFile.Load("user://Puzzle_" + Path.GetFileNameWithoutExtension(spPuzzleDataName) + ".cfg");
        return err == Error.Ok;
    }

    public void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        SPPuzzleEvent puzzleEvent = SPPDFImporter.LoadFromPDFData(body);
        puzzleEvent.keyName = _resultPuzzleName;
        // Cache this new on
        if (!_events.ContainsKey(_resultPuzzleName))
        {
            _events.Add(_resultPuzzleName, puzzleEvent);
        }
        else
        {
            _events[_resultPuzzleName] = puzzleEvent;
        }
        
        _callBack(puzzleEvent);
        puzzleEvent.Save();
        Save();
    }

    public void Save()
    {
        ConfigFile configFile = new ConfigFile();

        JSONArray jArray = new JSONArray();

        foreach (KeyValuePair<string, SPPuzzleEvent> pair in _events)
        {
            jArray.Add("SPName", pair.Key);
        }

        configFile.SetValue("SPData", "SPNameArray", jArray.ToString());

        configFile.Save("user://SPPuzzlerData.cfg");
    }

    public void Load()
    {
        _events.Clear();

        //return;

        ConfigFile configFile = new ConfigFile();
        Error err = configFile.Load("user://SPPuzzlerData.cfg");

        // If the file didn't load, ignore it.
        if (err != Error.Ok)
        {
            return;
        }

        string jStr = (string)configFile.GetValue("SPData", "SPNameArray");
        JSONNode jRoot = JSON.Parse(jStr);

        if (jRoot != null && jRoot.Count > 0)
        {
            for (int i = 0; i < jRoot.Count; i++)
            {
                string puzzleName = (string)jRoot[i];
                _events.Add(puzzleName, null);
            }
        }

    }

    public void GetSPCacheFromWeb(HttpRequest request, Action callback)
    {
        _callBackEventsUpdated = callback;
        request.Request(resultsPage);
    }

    public void OnRequestReaultsCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        string html = body.GetStringFromUtf8();

        string[] lines = html.Split(new[] { '\r', '\n' });

        for (int x = 0; x < lines.Length; x++) 
        {
            string line  = lines[x];
            if (line.ToLower().Contains("pdf"))
            {
                string sub = line.Substring(line.IndexOf("/"));
                string filename = SPPrefix + sub.Substring(0, sub.IndexOf("'"));
                //GD.Print(filename);

                if (!_events.ContainsKey(filename))
                {
                    _events.Add(filename, null);    
                }
            }
        }

        Save();        
        _callBackEventsUpdated();
    }

    public List<string> GetAllPuzzleNames()
    {
        List<string> names = new List<string>();

        foreach (KeyValuePair<string, SPPuzzleEvent> pair in _events)
        {
            // make this easier to read
            string name = pair.Key.Substring(pair.Key.LastIndexOf("/"));
            name = Path.GetFileNameWithoutExtension(name);
            int rIndex = name.IndexOf("_results");
            if (rIndex != -1)
            {
                name = name.Substring(0, rIndex);
            }
            names.Add(name);
        }

        //names = names.OrderBy(o => o).ToList();
        return names;
    }

    public string GetFullAsset(string prettyName)
    {
        foreach (KeyValuePair<string, SPPuzzleEvent> pair in _events)
        {
            if (pair.Key.Contains(prettyName))
            {
                return pair.Key;
            }
        }

        return "";
    }

    public bool CanUpdate()
    {
        return true;
    }*/
}
