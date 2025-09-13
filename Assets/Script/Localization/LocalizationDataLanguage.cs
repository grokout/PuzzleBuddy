using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationDataLanguage : ScriptableObject
{
    public string translationCode { set { _translationCode = value; } get { return _translationCode; } }
    [SerializeField] private string _translationCode;
    public List<string> cultures { get { if (_cultures == null) _cultures = new List<string>(); return _cultures; } }
    [SerializeField] private List<string> _cultures;

    public List<LocalizationDataEntry> _entries = new List<LocalizationDataEntry>();

    // chache
    private Dictionary<string, LocalizationDataEntry> _terms = new Dictionary<string, LocalizationDataEntry>();

    public Dictionary<string, LocalizationDataEntry> terms
    {
        get
        {
            if (_terms.Count == 0)
            {
                // Recache
                foreach (LocalizationDataEntry localizationDataEntry in _entries)
                {
                    _terms.Add(localizationDataEntry.key, localizationDataEntry);
                }

                //Debug.Log("Chached " + _terms.Count);
            }

            return _terms;
        }
    }

    public void AddKey(string key)
    {       
        if (!terms.ContainsKey(key))
        {
            LocalizationDataEntry localizationDataEntry = new LocalizationDataEntry();
            localizationDataEntry.key = key;
            _entries.Add(localizationDataEntry);
            _terms.Add(localizationDataEntry.key, localizationDataEntry);
        }
    }

    public void RemoveKey(string key)
    {
        _entries.Remove(terms[key]);
        terms.Remove(key);
    }

    public void Clear()
    {
        _entries.Clear();
        _terms.Clear();
    }

    public string GetValue(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("Invalid key");
            return "MISSING";
        }
        if (terms.ContainsKey(key))
        {
            return terms[key].value;
        }
        Debug.LogError("Missing key" + key);
        return "MISSING";
    }
}
