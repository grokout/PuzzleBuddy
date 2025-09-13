using System;
using System.Collections;
using System.Collections.Generic;

public class EventMsgManager : Singleton<EventMsgManager>
{
    public enum GameEventIDs
    {
        LanguageChanged,
        UpdateBrands,
        UpdatedDisplayName,
        FriendsUpdated,
        UpdateEntriesForPuzzle,
        SortFiltersChanged,
        LoginSuccess,
        LoginFailed,
        RegisterFailed,
        UpdatedFriendDisplayName,
        EntryRemoved,
        FastestTimeFound,
        LoadComplete,
        BarcodeScanned,
        BrandChanged,
        CountChanged,
        TextEntered,
        SortUpdated
    }

    public class GameEventArgs
    {
    }

    public class FastestTimeFoundArgs : GameEventArgs
    {
        public int userId;
        public float time;
        public DateTime date;

        public FastestTimeFoundArgs(int userId, float time, DateTime date)
        {
            this.userId = userId;
            this.time = time;
            this.date = date;
        }
    }

    public class ErrorArgs : GameEventArgs
    {
        public string msg;

        public ErrorArgs(string msg)
        {
            this.msg = msg;
        }
    }

    public class PuzzleArgs : GameEventArgs
    {
        public PBPuzzle pBPuzzle;

        public PuzzleArgs(PBPuzzle pBPuzzle)
        {
            this.pBPuzzle = pBPuzzle;
        }
    }

    public class UpdatedFriendDisplayNameArgs : GameEventArgs
    {
        public int id;

        public UpdatedFriendDisplayNameArgs(int id)
        {
            this.id = id;
        }
    }

    public class BarCodeScannedArgs : GameEventArgs
    {
        public string scanData;
    }

    public class BrandArgs : GameEventArgs
    {
        public Brand brand;
        public BrandArgs(Brand brand)
        {
            this.brand = brand;
        }
    }

    public class CountArgs : GameEventArgs
    {
        public int count;
        public CountArgs(int count)
        {
            this.count = count;
        }
    }
    public class TextEnteredArgs : GameEventArgs
    {
        public string textEntered;

        public TextEnteredArgs(string textEntered)
        {
            this.textEntered = textEntered;
        }   
    }

    public delegate void GameEventCallback(GameEventArgs inEventArge);
    Hashtable m_Events = new Hashtable();

    public bool AddListener(GameEventIDs inEventID, GameEventCallback inCallback)
    {
        if (inCallback == null)
            return false;

        if (!m_Events.Contains(inEventID))
            m_Events[inEventID] = new GameEvent();

        GameEvent gameEvent = m_Events[inEventID] as GameEvent;
        gameEvent.Add(inCallback);

        return true;
    }

    public bool RemoveListener(GameEventIDs inEventID, GameEventCallback inCallback)
    {
        if (inCallback == null)
            return false;

        if (!m_Events.Contains(inEventID))
            return false;

        GameEvent gameEvent = m_Events[inEventID] as GameEvent;
        gameEvent.Remove(inCallback);

        return true;
    }

    public void RemoveAllListenersForEvent(GameEventIDs eventID)
    {
        if (!m_Events.Contains(eventID))
            return;

        m_Events.Remove(eventID);
    }

    public void RemoveAllListeners()
    {
        m_Events.Clear();
    }

    public bool SendEvent(GameEventIDs inEventID, GameEventArgs inArgs = null)
    {
        if (!m_Events.Contains(inEventID))
            return false;

        GameEvent gameEvent = m_Events[inEventID] as GameEvent;
        gameEvent.Call(inArgs);

        return true;
    }

    class GameEvent
    {
        private event GameEventCallback m_EventHandler;

        public void Add(GameEventCallback inCallback)
        {
            m_EventHandler += inCallback;
        }

        public void Remove(GameEventCallback inCallback)
        {
            m_EventHandler -= inCallback;
        }

        public void Call(GameEventArgs inArgs = null)
        {
            if (m_EventHandler != null)
                m_EventHandler(inArgs);
        }
    }
}

