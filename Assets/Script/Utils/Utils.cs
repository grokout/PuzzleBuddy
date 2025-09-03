using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

static public class Utils
{
    public static string GetString(Dictionary<string, string> inObjectProps, string inKey, string inDefault = "")
    {
        string Ret = inDefault;
        if (inObjectProps.ContainsKey(inKey))
        {
            Ret = inObjectProps[inKey];
            if (Ret == "-")
            {
                Ret = "";
            }
        }
        return Ret;
    }

    public static float GetFloat(Dictionary<string, string> inObjectProps, string inKey, float inDefault = 0)
    {
        float Ret = inDefault;
        if (inObjectProps.ContainsKey(inKey))
        {
            string value = inObjectProps[inKey];
            Ret = float.Parse(value);
        }
        return Ret;
    }

    public static int GetInt(Dictionary<string, string> inObjectProps, string inKey, int inDefault = 0)
    {
        int Ret = inDefault;
        if (inObjectProps.ContainsKey(inKey))
        {
            string value = inObjectProps[inKey];
            if (!int.TryParse(value, out Ret))
            {
                Debug.LogError("Failed to parse " + inKey + "[" + value + "]");
            }
            Ret = int.Parse(value);
        }
        return Ret;
    }

    public static T GetEnum<T>(Dictionary<string, string> inObjectProps, string inKey, T inDefault)
    {
        T ret = inDefault;
        if (inObjectProps.ContainsKey(inKey))
        {
            string value = Utils.GetString(inObjectProps, inKey);
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    ret = (T)Enum.Parse(typeof(T), value);
                }
                catch
                {
                    Debug.LogError("Failed to Parse enum:" + value);
                }
            }
        }

        return ret;
    }

    public static bool GetBool(Dictionary<string, string> inObjectProps, string inKey, bool inDefault = false)
    {
        bool Ret = inDefault;
        if (inObjectProps.ContainsKey(inKey))
        {
            string value = inObjectProps[inKey];
            Ret = bool.Parse(value.ToLower());
        }
        return Ret;
    }

    public static Vector3 Vector3FromString(String s)
    {
        var stringArray = s.Substring(1, s.Length - 2);
        string[] parts = stringArray.Split(new string[] { "," }, StringSplitOptions.None);
        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2]));
    }

    public static Transform SearchForChild(Transform current, string name)
    {
        // check if the current is the object we're looking for, if so return it
        if (current.name == name)
            return current;

        // search through child objects for the one we're looking for
        for (int i = 0; i < current.childCount; ++i)
        {
            // the recursive step; repeat the search one step deeper in the hierarchy
            Transform found = SearchForChild(current.GetChild(i), name);
            // a transform was returned by the search above that is not null,
            // it must be the one we're looking for
            if (found != null)
                return found;
        }

        // not found
        return null;
    }

    public static GameObject SearchForChildObj(Transform current, string name)
    {
        Transform trans = SearchForChild(current, name);
        if (trans != null)
        {
            return trans.gameObject;
        }
        return null;
    }

    public static string GetReadableTime(double seconds)
    {
        string ret = "";

        if (seconds < 10)
        {
            ret = seconds.ToString("N2") + "S";
        }
        else if (seconds <= 60)
        {
            ret = seconds.ToString("N0") + "S";
        }
        else if (seconds <= 3600)
        {
            ret = (seconds / 60).ToString("N0") + "M";
        }
        else if (seconds <= (3600 * 24))
        {
            ret = (seconds / 3600).ToString("N0") + "H";
        }
        else 
        {
            ret = (seconds / (3600 * 34)).ToString("N0") + "Y";
        }
        return ret;
    }

    public static string GetReadableTime(TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays / 356 >= 1)
        {
            return (timeSpan.TotalDays / 356).ToString("N0") + "Y";
        }
        else if (timeSpan.TotalDays >= 1)
        {
            return timeSpan.TotalDays.ToString("N0") + "D";
        }
        else if (timeSpan.TotalHours >= 1)
        {
            return timeSpan.TotalHours.ToString("N0") + "H";
        }
        else if (timeSpan.TotalMinutes >= 1)
        {
            return timeSpan.TotalMinutes.ToString("N0") + "M";
        }
        else 
        {
            return timeSpan.TotalSeconds.ToString("N0") + "S";
        }  
    }

    public static string GetShorthandStringForValue(double value)
    {
        List<string> _largeNumberAppends = new List<string>() { "", "K", "M", "B", "T", "Qa", "Qi", "S", "Sp", "O", "N" };
        string append = "";

        for (int x = 7; x >= 0; --x)
        {
            double comp = 1;
            for (int y = 0; y < x; y++)
            {

                comp *= 1000;
            }

            if (value > (comp - 1))
            {
                value /= comp;
                value = ((int)Math.Round(value * 10)) / 10f;
                append = _largeNumberAppends[x];
                break;
            }
        }

        return value.ToString() + append;
    }

}