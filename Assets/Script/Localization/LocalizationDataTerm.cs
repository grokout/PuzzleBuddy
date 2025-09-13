using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationDataTerm 
{
    public enum DataType
    {
        Phrase,
        Sprite,
        GameObject
    }

    public string key;
    public DataType dataType = DataType.Phrase;


    [SerializeField]
    private string _keyCat;
    [SerializeField]
    private string _keyValue;

    public LocalizationDataTerm(string key)
    {
        this.key = key;
        _keyCat = "";
        _keyValue = key;
        int i = key.LastIndexOf("/");
        if (i != -1)
        {
            _keyCat = key.Substring(0, i);
            _keyValue = key.Substring(i + 1); 
        }
    }

    public string GetKeyCat()
    {
        return _keyCat;
    }

    public string GetKeyNoCat()
    {
        return _keyValue;
    }
}
