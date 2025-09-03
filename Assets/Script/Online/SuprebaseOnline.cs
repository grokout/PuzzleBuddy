using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;

public class SuprebaseOnline : SingletonMonoBehaviour<SuprebaseOnline>
{
    const string SUPABASE_PUBLIC_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVobGxpcnhpeG5kbXFtb2tyYW5vIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjA5NTU1NzQsImV4cCI6MjAzNjUzMTU3NH0.fh56aWcv9Z05sH_LKavE1tLz_EnBW6N6PARjWQtJc0M";
    const string PROJECT_ID = "uhllirxixndmqmokrano";
    const string URL = "https://uhllirxixndmqmokrano.supabase.co/auth/v1";

    const string DBURL = "https://uhllirxixndmqmokrano.supabase.co/rest/v1/rpc/";

    public List<RequestData> _requestQueue = new List<RequestData>();

    public enum RequestType
    {
        Submit,
        GetBrand,
        GetTimeIds,
        FetchEntry,
        DeleteEntry,
        UpdateEntry,
        GerUserEntriesForPuzzle,
        Test,
        RegisterNewUser,
        LoginUser,
        GetDisplayName,
        SetDisplayName,
        AddFriend,
        RemoveFriend,
        GetFriends,
        GetFastestTime
    }

    public void Setup()
    {
    }

    /*private void FetchEntryRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Debug.Log("OnFetchEntry Complete");
        string msg = body.GetStringFromUtf8();
        Debug.Log(msg);

        JSONObject jSon = new JSONObject(msg);

        PBPuzzleManager.instance.OnFetchEntry(jSon);
    }*/

    void AddResultQueue(RequestType requestType, string url, string[] customHeaders = null, 
                            bool methodPost = false, string requestData = "", int userId = 0)
    {
        RequestData requestDataObj = new RequestData()
        {
            requestType = requestType,
            url = url,
            customHeaders = customHeaders,
            methodPost = methodPost,
            requestData = requestData,
            userId = userId
        };

        AddResultQueue(requestDataObj);
    }
    
    void AddResultQueue(RequestData requestDataObj)
    {
        bool kickOff = false;
        if (_requestQueue.Count == 0)
        {
            kickOff = true;
        }
        
        _requestQueue.Add(requestDataObj);
        if (kickOff)
        {
            KickOffNextInQueue();
        }
    }

    void KickOffNextInQueue()
    {
        Debug.Log("Queue " + _requestQueue[0].requestType);
        if (_requestQueue[0].methodPost)
        {
            StartCoroutine(PostRequest(_requestQueue[0]));
        }
        
    }

    IEnumerator PostRequest(RequestData requestDataObj)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(requestDataObj.url, requestDataObj.requestData, "application/json"))
        {
            webRequest.SetRequestHeader("apikey",SUPABASE_PUBLIC_KEY);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Handle the response
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending request: " + webRequest.error);
                Debug.LogError(requestDataObj.url);
                Debug.LogError(requestDataObj.requestData);
            }
            else
            {
                Debug.Log("Request successful!");
                OnRequestCompleted(webRequest.downloadHandler.text);
            }
        }

    }

    public void OnRequestCompleted(string body)
    {
        Debug.Log("OnRequestCompleted - " + body);

        switch (_requestQueue[0].requestType)
        {
            case RequestType.GetBrand:
                BrandManager.instance.OnRequestCompletedBrandList(body);
                break;
            case RequestType.LoginUser:
                {
                    string ret = body;
                    if (ret.ToLower().Contains("error"))
                    {
                        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.LoginFailed, new EventMsgManager.ErrorArgs(ret));
                    }
                    else
                    {
                        ret = ret.Replace("\"", "").Replace(" ", "");
                        int.TryParse(ret, out OnlineManager.instance.myAccount.userId);
                        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.LoginSuccess);
                    }
                }
                break;
            case RequestType.GetTimeIds:
                PBPuzzleManager.instance.OnUpdatedUserTimeIds(body);
                break;
            case RequestType.FetchEntry:
                PBPuzzleManager.instance.OnFetchEntry(body);
                break;
        }


        _requestQueue.RemoveAt(0);
        if (_requestQueue.Count > 0)
        {
            KickOffNextInQueue();
        }
    }
    /*
    public void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        GD.Print("OnRequestCompleted");
     
        switch (_requestQueue[0].requestType)
        {


            case RequestType.Submit:
                {
                    string bdy = body.GetStringFromUtf8();
                    string record = bdy.Replace("(", "").Replace(")", "").Replace("\"", "").Replace("\\", "");
                    List<string> cols = record.Split(',').ToList<string>();
                    int dbId = -1;
                    int.TryParse(cols[0], out dbId);    
                    _requestQueue[0].pBEntry.SetDBId(dbId);
                }
                break;
            case RequestType.UpdateEntry:
                GD.Print(body.GetStringFromUtf8());
                break;
            case RequestType.GerUserEntriesForPuzzle:
                PBPuzzleManager.instance.OnUpdatedUserEntriesForPuzzle(body.GetStringFromUtf8());
                break;
            case RequestType.Test:
                GD.Print(body.GetStringFromUtf8());
                break;
            case RequestType.RegisterNewUser:
                {
                    string ret = body.GetStringFromUtf8();
                    if (ret.ToLower().Contains("error"))
                    {
                        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.RegisterFailed, new EventMsgManager.ErrorArgs(ret));
                    }
                    else
                    {
                        ret = ret.Replace("\"", "").Replace(" ", "");
                        int.TryParse(ret, out OnlineManager.instance.myAccount.userId);
                        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.LoginSuccess);
                    }
                }
                break;

            case RequestType.GetDisplayName:
                {
                    string ret = body.GetStringFromUtf8();
                    ret = ret.Replace("\\", "").Replace("\"", "").Replace(" ", "").Replace("(", "").Replace(")", "");
                    OnlineManager.instance.SetDisplayName(_requestQueue[0].userId, ret);
                    GD.Print(ret);
                }
                break;
            case RequestType.SetDisplayName:
                { 
                    string ret = body.GetStringFromUtf8();
                    GD.Print(ret);
                }
                break;
            case RequestType.AddFriend:
                {
                    string ret = body.GetStringFromUtf8();
                    GD.Print(ret);
                    OnlineManager.instance.onlineFriends.AddFriendReturn(ret);
                }
                break;
            case RequestType.RemoveFriend:
                {
                    string ret = body.GetStringFromUtf8();
                    GD.Print(ret);
                }
                break;
            case RequestType.GetFriends:
                {
                    string ret = body.GetStringFromUtf8();
                    GD.Print(ret);
                    OnlineManager.instance.onlineFriends.OnGetFriends(ret);
                }
                break;
                case RequestType.GetFastestTime:
                {
                    string ret = body.GetStringFromUtf8();
                    ret = ret.Replace("\\", "").Replace("\"", "").Replace(" ", "").Replace("(", "").Replace(")", "");
                    GD.Print(ret);
                    List<string> cols = ret.Split(',').ToList<string>();

                    float time = 0;
                    float.TryParse(cols[0], out time);
                    int userId = 0;
                    int.TryParse(cols[1], out userId);
                    DateTime date = DateTime.Now;
                    DateTime.TryParse(cols[2], out date);

                    EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.FastestTimeFound, new EventMsgManager.FastestTimeFoundArgs(userId, time, date));
                }
                break;
        }

        _requestQueue.RemoveAt(0);
        if (_requestQueue.Count > 0)
        {
            KickOffNextInQueue();
        }
        
    }

    public void AddEntry(PBEntry entry)
    {
        GD.Print("DB AddEntry");

        string userId = OnlineManager.instance.GetUserId().ToString();
        string jsons = "{\"userid\": \""+ userId + "\",\"b_dnf\": "+ (entry.dnf ? 1 : 0) + ",\"n_placed\": "+ entry.placed.ToString() + ",\"f_time\": " + entry.GetTime().ToString() + ",\"d_date\": \"" + entry.date.ToString() + "\",\"t_puzzle_brand\": \"" + entry.pBPuzzle.brand + "\",\"t_puzzle_name\": \"" + entry.pBPuzzle.name + "\",\"n_puzzle_piece_count\": \"" + entry.pBPuzzle.pieceCount.ToString() + "\",\"t_teammate_1\": \"" + entry.GetTeamMemberName(0) + "\",\"t_teammate_2\": \"" + entry.GetTeamMemberName(1) + "\",\"t_teammate_3\": \"" + entry.GetTeamMemberName(2) + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "SubmitPBEntry2";

        GD.Print(jsons);

        RequestData requestDataObj = new RequestData()
        {
            requestType = RequestType.Submit,
            url = url,
            customHeaders = headers,
            method = Godot.HttpClient.Method.Post,
            requestData = jsons,
            pBEntry = entry
            
        };

        AddResultQueue(requestDataObj);       
    }

    public void UpdateEntry(PBEntry entry)
    {
        GD.Print("DB UpdateEntry");

        string userId = OnlineManager.instance.GetUserId().ToString();
        string jsons = "{\"eid\": " + entry.dbId + ",  \"userid\": \"" + userId + "\",\"b_dnf\": " + (entry.dnf ? 1 : 0) + ",\"n_placed\": " + entry.placed.ToString() + ",\"f_time\": " + entry.GetTime().ToString() + ",\"d_date\": \"" + entry.date.ToString() + "\",\"s_team\": \"" + entry.GetTeamMembersJson() + "\",\"t_puzzle_brand\": \"" + entry.pBPuzzle.brand + "\",\"t_puzzle_name\": \"" + entry.pBPuzzle.name + "\",\"n_puzzle_piece_count\": \"" + entry.pBPuzzle.pieceCount.ToString() + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "UpdatePBEntry";

        GD.Print(jsons);

        RequestData requestDataObj = new RequestData()
        {
            requestType = RequestType.UpdateEntry,
            url = url,
            customHeaders = headers,
            method = Godot.HttpClient.Method.Post,
            requestData = jsons,
            pBEntry = entry

        };

        AddResultQueue(requestDataObj);
    }*/

    public void FetchEntry(int dbId)
    {
        Debug.Log("DB FetchEntry");

        string userId = OnlineManager.instance.GetUserId();
        string jsons = "{\"entryid\": " + dbId + "}";
        string[] headers = new string[] { };
        string url = DBURL + "FetchPBEntry";
        AddResultQueue(RequestType.FetchEntry,url, headers, true, jsons);
    } 

    public void UpdateOnlineBrandList()
    {
        Debug.Log("UpdateOnlineBrandList");
        string[] headers = new string[] { };
        string url = DBURL + "FetchBrands";
        AddResultQueue(RequestType.GetBrand,url, headers, true);
    }
    
    public void UpdateUserTimeIds(string userId)
    {
        Debug.Log("UpdateUserTimeIds");

        string jsons = "{\"userid\": \"" + userId + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "GetEventIDs";

        AddResultQueue( RequestType.GetTimeIds,url, headers, true, jsons);
    }

    /*
    public void RemoveEntry(int dbId)
    {
        GD.Print("RemoveEntry");
        string userId = OnlineManager.instance.GetUserId();
        string jsons = "{\"entryid\": " + dbId + "}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "RemoveEntry";
        AddResultQueue(RequestType.DeleteEntry, url, headers, Godot.HttpClient.Method.Post, jsons);
    }

    public void GerUserEntriesForPuzzle(string userId, string brandName, string puzzleName, int pieceCount)
    {
        GD.Print("GerUserEntriesForPuzzle " + userId);

        string jsons = "{\"userid\": \"" + userId + "\",\"t_puzzle_brand\": \"" + brandName + "\",\"t_puzzle_name\": \"" + puzzleName + "\",\"n_puzzle_piece_count\": \"" + pieceCount.ToString() + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "GerUserEntriesForPuzzle";

        GD.Print(jsons);
        AddResultQueue(RequestType.GerUserEntriesForPuzzle, url, headers, Godot.HttpClient.Method.Post, jsons);
    }

    public void Test()
    {
        GD.Print("Test");

        string jsons = "";// "{\"userid\": \"" + userId + "\",\"t_puzzle_brand\": \"" + brandName + "\",\"t_puzzle_name\": \"" + puzzleName + "\",\"n_puzzle_piece_count\": \"" + pieceCount.ToString() + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "tests";

        //GD.Print(jsons);
        AddResultQueue(RequestType.Test, url, headers, Godot.HttpClient.Method.Post, jsons);
    }

    public void RegisterNewUser(string email, string password)
    {
        string jsons = "{\"email\": \"" + email + "\",\"password\": \"" + password + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "RegisterNewUser";

        AddResultQueue(RequestType.RegisterNewUser, url, headers, Godot.HttpClient.Method.Post, jsons);
    }*/

    public void LoginUser(string email, string password)
    {
        string jsons = "{\"email\": \"" + email + "\",\"password\": \"" + password + "\"}";
        string[] headers = new string[] {  };
        string url = DBURL + "LoginUser";

        AddResultQueue(RequestType.LoginUser, url, headers, true, jsons);
    }

    /*public void SetDisplayName(string displayName)
    {
        string jsons = "{\"userid\": " + OnlineManager.instance.GetUserIdAsInt() + ",\"displayname\": \"" + displayName + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "SetDisplayName";

        AddResultQueue(RequestType.SetDisplayName, url, headers, Godot.HttpClient.Method.Post, jsons);
    }

    public void GetDisplayName(int userId)
    {
        string jsons = "{\"userid\": " + userId + "}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "GetDisplayName";

        AddResultQueue(RequestType.GetDisplayName, url, headers, Godot.HttpClient.Method.Post, jsons, userId);
    }

    public void AddFriend(string friendId)
    {
        string jsons = "{\"userid\": " + OnlineManager.instance.GetUserIdAsInt() + ",\"friendid\": \"" + friendId + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "AddFriend";

        AddResultQueue(RequestType.AddFriend, url, headers, Godot.HttpClient.Method.Post, jsons);
    }

    public void RemoveFriend(FriendData friendData)
    {
        string jsons = "{\"userid\":  \"" + OnlineManager.instance.GetUserId() + "\",\"friendid\": \"" + friendData.id + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "RemoveFriend";

        AddResultQueue(RequestType.RemoveFriend, url, headers, Godot.HttpClient.Method.Post, jsons);        
    }

    public void GetFriends()
    {
        string jsons = "{\"userid\":  \"" + OnlineManager.instance.GetUserId() + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "GetFriends";

        AddResultQueue(RequestType.GetFriends, url, headers, Godot.HttpClient.Method.Post, jsons);
    }

    public void GetFastestTime(PBPuzzle pBPuzzle)
    {
        string jsons = "{\"brandname\":  \"" + pBPuzzle.brand + "\",\"puzzlename\":  \"" + pBPuzzle.name + "\",\"piececount\":  \"" + pBPuzzle.pieceCount + "\"}";
        string[] headers = new string[] { "apikey: " + SUPABASE_PUBLIC_KEY };
        string url = DBURL + "GetFastestTime";

        AddResultQueue(RequestType.GetFastestTime, url, headers, Godot.HttpClient.Method.Post, jsons);
    }*/
}


public class RequestData
{
    public string url;
    public string[] customHeaders = null;
    public bool methodPost = false;
    public string requestData = "";
    public SuprebaseOnline.RequestType requestType = SuprebaseOnline.RequestType.GetBrand;
    public PBEntry pBEntry;
    public int userId;
}