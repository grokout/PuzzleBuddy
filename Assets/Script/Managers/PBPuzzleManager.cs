using Defective.JSON;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PBPuzzleManager : Singleton<PBPuzzleManager>
{
    private Dictionary<string, Dictionary<string, Dictionary<int, PBPuzzle>>> _puzzles = new Dictionary<string, Dictionary<string, Dictionary<int, PBPuzzle>>>();
    
   // private ConfigFile _configFile = new ConfigFile();


    public Dictionary<string, Dictionary<string, Dictionary<int, PBPuzzle>>> puzzles { get {  return _puzzles; } }

   

    private int _fetchCount = 0;
    public PuzzleSortType puzzleSortType = PuzzleSortType.EnterD;

    string GetSearchName(string name)
    {
        return name.ToLower().Replace(" ", "");;
    }

    public void AddTime(PBEntry entry, bool save = true)
    {
        if (entry == null || string.IsNullOrEmpty(entry.puzzleName))
        {
            Debug.LogError("Missing entry");
            return;
        }
        string searchName = GetSearchName(entry.puzzleName);
        if (!_puzzles.ContainsKey(entry.brand))
        {
            _puzzles.Add(entry.brand, new Dictionary<string, Dictionary<int, PBPuzzle>>());
        }

        if (!_puzzles[entry.brand].ContainsKey(searchName))
        {
            _puzzles[entry.brand].Add(searchName, new Dictionary<int, PBPuzzle>()) ;
        }

        if (!_puzzles[entry.brand][searchName].ContainsKey(entry.puzzleUpc))
        {
            _puzzles[entry.brand][searchName].Add(entry.puzzleUpc, new PBPuzzle(entry));
            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdateBrands);
        }

        _puzzles[entry.brand][searchName][entry.puzzleUpc].AddTime(entry);

        if (save)
        {
            if (entry.userId == OnlineManager.instance.GetUserId().ToString())
            {
                Save();
            }
        }
    }


    /*
    public void RemovePuzzle(PBPuzzle pBPuzzle)
    {
        if (!_puzzles.ContainsKey(pBPuzzle.brand))
        {
            return;
        }

        if (_puzzles[pBPuzzle.brand].ContainsKey(pBPuzzle.name))
        {
            _puzzles[pBPuzzle.brand].Remove(pBPuzzle.name);
            if (_puzzles[pBPuzzle.brand].Count == 0)
            {
                _puzzles.Remove(pBPuzzle.brand);
                EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdateBrands);
            }
        }        
    
        Save();
    }
    */
    public void Load()
    {
        
        string jSonStr = PlayerPrefs.GetString("Puzzles","");
        if (string.IsNullOrEmpty(jSonStr))
        {
            return;
        }

        JSONObject jRoot = new JSONObject(jSonStr);

        foreach (JSONObject jPuzzle in jRoot.list)
        {
            PBPuzzle pBPuzzle = new PBPuzzle();
            pBPuzzle.Load(jPuzzle);

            if (string.IsNullOrEmpty(pBPuzzle.name))
            {
                continue;
            }
            string searchName = GetSearchName(pBPuzzle.name);
            if (!_puzzles.ContainsKey(pBPuzzle.brand))
            {
                _puzzles.Add(pBPuzzle.brand, new Dictionary<string, Dictionary<int, PBPuzzle>>());
            }

            if (!_puzzles[pBPuzzle.brand].ContainsKey(searchName))
            {
                _puzzles[pBPuzzle.brand].Add(searchName, new Dictionary<int, PBPuzzle>());
            }

            if (!_puzzles[pBPuzzle.brand][searchName].ContainsKey(pBPuzzle.upc))
            {
                _puzzles[pBPuzzle.brand][searchName].Add(pBPuzzle.upc, pBPuzzle);                
            }
            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdateEntriesForPuzzle, new EventMsgManager.PuzzleArgs(pBPuzzle));
        }

        int i = PlayerPrefs.GetInt("puzzleSortType");
        puzzleSortType = (PuzzleSortType)i;
    }

    public void Save()
    {
        JSONObject jArray = new JSONObject(JSONObject.Type.Array);

        foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, PBPuzzle>>> brandPair in _puzzles)
        {
            foreach (KeyValuePair<string, Dictionary<int, PBPuzzle>> namePair in brandPair.Value)
            {
                foreach (KeyValuePair<int, PBPuzzle> upcPair in namePair.Value)
                {
                    jArray.Add(upcPair.Value.Serialize());
   
                }
            }
        }

        PlayerPrefs.SetString("Puzzles", jArray.ToString());
        PlayerPrefs.SetInt("puzzleSortType", (int)puzzleSortType);
    }

    /*
    public void FixDisplayNames()
    {
        foreach (KeyValuePair<string, Dictionary<string, PBPuzzle>> pair in _puzzles)
        {
            foreach (KeyValuePair<string, PBPuzzle> namePair in pair.Value)
            {
                foreach (PBEntry pBEntry in namePair.Value.entries)
                {
                    pBEntry.userId = OnlineManager.instance.GetUserId().ToString();
                }
            }
        }

        Save();
    }
    */
    public void LoadDB()
    {
        // Check online for events we have saved
        SuprebaseOnline.instance.UpdateUserTimeIds(OnlineManager.instance.GetUserId().ToString());
        
        // Check if we have any entries that need to be uploaded
        
    }

    public void OnUpdatedUserTimeIds(string body)
    {
        Debug.Log("OnUpdatedUserTimeIds Complete");
 
        Debug.Log(body);

        if (string.IsNullOrEmpty(body))
        {
            return;
        }
        List<int> dbIds = new List<int>();

        JSONObject jSon = new JSONObject(body);
        if (jSon != null && jSon.list != null)
        {
            foreach (JSONObject jN in jSon.list)
            {
                int id = jN.intValue;
                dbIds.Add(id);
            }
        }

        // check if any of our offline entries should be uploaded
        foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, PBPuzzle>>> brandPair in _puzzles)
        {
            foreach (KeyValuePair<string, Dictionary<int, PBPuzzle>> namePair in brandPair.Value)
            {
                foreach (KeyValuePair<int, PBPuzzle> upcPair in namePair.Value)
                {
                    foreach (PBEntry pBEntry in upcPair.Value.entries)
                    {
                        if (pBEntry.dbId == -1 || !dbIds.Contains(pBEntry.dbId))
                        {
                            Debug.Log("Uploading offline Entry");
                            // TODO
                            //SuprebaseOnline.instance.AddEntry(pBEntry);
                        }
                    }
                }
            }
        }

        // then check if we need to grab any from the db

        bool shownStatus = false;
        foreach (int id in dbIds)
        {
            if (!HasDBId(id))
            {
                Debug.Log("Grabbing from DB " + id);
                FetchDBEntry(id);
                _fetchCount++;
                if (!shownStatus)
                {
                    shownStatus = true;
                    UIManager.instance.ShowPanel("UIStatus", new UIStatusData("Updating from database"));
                }
            }
        }
    }

    public void OnUpdatedUserEntriesForPuzzle(string strJson)
    {
        
        JSONObject json = new JSONObject(strJson);

        if (json != null)
        {
            if (json != null && json.list != null && json.list.Count > 0)
            {
                foreach (JSONObject jsonN in json.list)
                {
                    Debug.Log(jsonN.intValue);
                    SuprebaseOnline.instance.FetchEntry(jsonN.intValue);
                }
            }
        

            /*    PBEntry pBEntry = new PBEntry();

                string brand = json["puzzle_brand"];
                string puzzleName = json["puzzle_name"];
                int puzzleCount = json["puzzle_piece_count"];

                pBEntry.LoadFromBD(json);
                AddTime(brand, puzzleName, puzzleCount, pBEntry);

                PBPuzzle pBPuzzle = GetPuzzle(brand, puzzleName);

                EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdateEntriesForPuzzle, new EventMsgManager.PuzzleArgs(pBPuzzle));*/
        }


    }

    bool HasDBId(int dbID)
    {
        // TODO optimize this
        foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, PBPuzzle>>> brandPair in _puzzles)
        {
            foreach (KeyValuePair<string, Dictionary<int, PBPuzzle>> namePair in brandPair.Value)
            {
                foreach (KeyValuePair<int, PBPuzzle> upcPair in namePair.Value)
                {
                    foreach (PBEntry pBEntry in upcPair.Value.entries)
                    {
                        if (pBEntry.dbId == dbID)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
    // Gets entry info from the DB
    void FetchDBEntry(int dbId)
    {
        SuprebaseOnline.instance.FetchEntry(dbId);
        // this means its on the server but not in the local database
    }
   
    public void OnFetchEntry(string record)
    {
        //Debug.Log("Entry - " + record);
        PBEntry pBEntry = new PBEntry();

        record = record.Replace("(", "").Replace(")", "").Replace("\"","").Replace("\\", "");
        List<string> cols = record.Split(',').ToList<string>();

        if (cols.Count >= 10)
        {
            pBEntry.LoadFromBD(cols);
            AddTime(pBEntry);

            PBPuzzle pBPuzzle = GetPuzzle(pBEntry.brand, pBEntry.puzzleName, pBEntry.puzzleUpc);
            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdateEntriesForPuzzle, new EventMsgManager.PuzzleArgs(pBPuzzle)); 
        }

        --_fetchCount;
        if (_fetchCount == 0)
        {
            UIManager.instance.HidePanel("UIStatus");
        }

    }

    public PBPuzzle GetPuzzle(PBEntry pBEntry)
    {
        return GetPuzzle(pBEntry.brand, pBEntry.puzzleName, pBEntry.puzzleUpc);
    }

    public PBPuzzle GetPuzzle(PBPuzzleCreationArgs pBPuzzleCreationArgs)
    {
        return GetPuzzle(pBPuzzleCreationArgs.brand, pBPuzzleCreationArgs.name, pBPuzzleCreationArgs.upc);
    }

    public PBPuzzle GetPuzzle(string brand, string name, int upc)
    {
        string searchName = GetSearchName(name);
        if (_puzzles.ContainsKey(brand))
        {
            if (_puzzles[brand].ContainsKey(searchName))
            {
                if (_puzzles[brand][searchName].ContainsKey(upc))
                {
                    return _puzzles[brand][searchName][upc];
                }
            }
        }

        return null;
    }

    /*public PBPuzzle GetRandomPuzzle()
    {
        return _puzzles.ElementAt((int)(GD.Randi() % _puzzles.Count)).Value.ElementAt(0).Value;
    }*/

    public bool HasPuzzles()
    {
        return _puzzles.Count > 0;
    
    
    }

    public List<string> GetBrands()
    {
        List<string> brands = new List<string>();

        foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, PBPuzzle>>> pair in _puzzles)
        {

            brands.Add(pair.Key);   
        }               

        return brands;
    }

    public List<string> GetPuzzleNamesInBrands(string brand)
    {
        List<string> names = new List<string>();

        if(_puzzles.ContainsKey(brand))
        {
            foreach (KeyValuePair<string, Dictionary<int, PBPuzzle>> pair in _puzzles[brand])
            {
                names.Add(pair.Key);
            }
        }        

        return names;
    }

    public void GetFriendEntriesFor(PBPuzzle pBPuzzle)
    {
        if (pBPuzzle.loadedFriendsData)
        {
            return;
        }
        pBPuzzle.loadedFriendsData = true;
        string searchName = GetSearchName(pBPuzzle.name);
        foreach (KeyValuePair<string, FriendData> friendData in OnlineManager.instance.onlineFriends.friends)
        {
            SuprebaseOnline.instance.GerUserEntriesForPuzzle(friendData.Value.id, pBPuzzle.brand, searchName, pBPuzzle.pieceCount);
        }
    }

    public void RemovePuzzlesBy(string id)
    {
        foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, PBPuzzle>>> brandPair in _puzzles)
        {
            foreach (KeyValuePair<string, Dictionary<int, PBPuzzle>> namePair in brandPair.Value)
            {
                foreach (KeyValuePair<int, PBPuzzle> upcPair in namePair.Value)
                {
                    List<PBEntry> removeList = new List<PBEntry>();
                    foreach (PBEntry pBEntry in upcPair.Value.entries)
                    {
                        if (pBEntry.userId == id)
                        {
                            removeList.Add(pBEntry);
                        }
                    }

                    foreach (PBEntry pBEntry1 in removeList)
                    {
                        upcPair.Value.entries.Remove(pBEntry1);
                    }
                }
            }
        }
    }

    /*public void RemovePuzzleBrand(string brand, string puzzleName)
    {
        PBPuzzle pBPuzzle = null;
        if (_puzzles.ContainsKey(brand))
        {
            if (_puzzles[brand].ContainsKey(puzzleName))
            {
                pBPuzzle = _puzzles[brand][puzzleName];
            }
        }

        if (pBPuzzle != null)
        {
            foreach (PBEntry pBEntry in pBPuzzle.entries)
            {
                SuprebaseOnline.instance.RemoveEntry(pBEntry.dbId);
            }
            _puzzles[brand].Remove(puzzleName);
        }
    }
    */
    
    /*public void UpdatePuzzleInfo(string oldBrand, string oldName, int oldCount, string newBrand, string newName, int newCount)
    {
        string searchNameOld = GetSearchName(oldName);
        string searchNameNew = GetSearchName(newName);

        PBPuzzle pBPuzzle = null;
        if (_puzzles.ContainsKey(oldBrand))
        {
            if (oldName != newName || oldCount != newCount)
            {
                if (_puzzles[oldBrand].ContainsKey(searchNameOld))
                {
                   pBPuzzle = _puzzles[oldBrand][searchNameOld]; 
 
                }
            }
        }

        if (pBPuzzle != null)
        {
            pBPuzzle.brand = newBrand;
            pBPuzzle.name = newName;
            pBPuzzle.pieceCount = newCount;


            _puzzles[oldBrand].Remove(searchNameOld);
            if (!_puzzles.ContainsKey(newBrand))
            {
                _puzzles.Add(newBrand, new Dictionary<string, PBPuzzle>());
            }

            _puzzles[newBrand].Add(searchNameNew, pBPuzzle);

            // need to update the Database
            foreach (PBEntry pBEntry in pBPuzzle.entries)
            {
                SuprebaseOnline.instance.UpdateEntry(pBEntry);    
            }

            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.UpdateBrands);
            pBPuzzle.Save();
            Save();
        }        
    }*/

    public List<int> GetCounts()
    {
        List<int> counts = new List<int>() { 200,300, 500, 750, 1000}; 

        

        return counts;
    }
}
