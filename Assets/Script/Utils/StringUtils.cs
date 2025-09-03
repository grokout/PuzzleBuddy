using System.Text.RegularExpressions;
using UnityEngine;

public static class StringUtils
{
    public static int GetNumberFromString(string str)
    {
        int num = -1;
        string resultString = Regex.Match(str, @"\d+").Value;

        int.TryParse(resultString, out num);

        return num;
    }

    public static string GetTimeFormated(float time)
    {
        int hour = (int)(time / 60);
        int minute = (int)(time - (60 * hour));
        int sec = Mathf.RoundToInt((time % 1) * 60);

        string tStr = hour.ToString() + ":" + minute.ToString() + ":" + sec.ToString();
        return tStr;       
    }
}
