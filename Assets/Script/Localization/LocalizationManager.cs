using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : ScriptableObject
{
    public List<LocalizationDataLanguage> languages = new List<LocalizationDataLanguage>();
    public List<LocalizationDataTerm> GetAllTermsI() { return _terms; }

    [SerializeField]
    private List<LocalizationDataTerm> _terms = new List<LocalizationDataTerm>();
    private Dictionary<string, LocalizationDataTerm> _termsChache = new Dictionary<string, LocalizationDataTerm>();

    [SerializeField]
    private string _currentLang = "en";
    private LocalizationDataLanguage _currentLocalizationDataLanguage;
    private static LocalizationManager _instance;

    public static LocalizationManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<LocalizationManager>("LocalizationManager");
            }
            return _instance;
        }
    }

    public Dictionary<string, LocalizationDataTerm> termsChache
    {
        get
        {
            if (_termsChache.Count == 0)
            {
                foreach (LocalizationDataTerm localizationDataTerm in _terms)
                {
                    _termsChache.Add(localizationDataTerm.key, localizationDataTerm);
                }
            }

            return _termsChache;
        }
    }
    public bool AddKey(string key)
    {
        if (termsChache.ContainsKey(key))
        {
            return false;
        }

        LocalizationDataTerm localizationDataTerm = new LocalizationDataTerm(key);
        _terms.Add(localizationDataTerm);
        _termsChache.Add(key, localizationDataTerm);

        foreach (LocalizationDataLanguage localizationDataLanguage in languages)
        {
            localizationDataLanguage.AddKey(key);
        }

        return true;
    }

    public void RemoveKey(LocalizationDataTerm localizationDataTerm)
    {
        _terms.Remove(localizationDataTerm);
        _termsChache.Remove(localizationDataTerm.key);
        foreach (LocalizationDataLanguage localizationDataLanguage in languages)
        {
            localizationDataLanguage.RemoveKey(localizationDataTerm.key);
        }
    }

    public void AddLanguage(LocalizationDataLanguage localizationDataLanguage)
    {
        languages.Add(localizationDataLanguage);
        // add all keys to this 
        foreach (LocalizationDataTerm localizationDataTerm in _terms)
        {
            localizationDataLanguage.AddKey(localizationDataTerm.key);
        }
    }

    public LocalizationDataLanguage GetLanguage(string cultureCode)
    {
        foreach (LocalizationDataLanguage localizationDataLanguage in languages)
        {
            if (localizationDataLanguage.cultures.Contains(cultureCode))
            {
                return localizationDataLanguage;
            }
        }

        return null;
    }

    public void Clear()
    {
        _terms.Clear();
        _termsChache.Clear();
        foreach (LocalizationDataLanguage localizationDataLanguage in languages)
        {
            localizationDataLanguage.Clear();
        }
    }

    public string GetTranslationI(string key)
    {
        if (_currentLocalizationDataLanguage == null)
        {
            _currentLocalizationDataLanguage = GetLanguage(_currentLang);
        }

        return _currentLocalizationDataLanguage.GetValue(key);
    }

    public static string GetTranslation(string key)
    {
        return instance.GetTranslationI(key);
    }

    public static bool HasKey(string key)
    {
        return instance.HasKeyI(key);
    }

    public bool HasKeyI(string key)
    {
        return termsChache.ContainsKey(key);
    }

    public static List<LocalizationDataTerm> GetAllTerms()
    {
        return instance.GetAllTermsI();
    }

    public void SwapKeys()
    {
        foreach (LocalizationDataTerm localizationDataTerm in _terms)
        {
            localizationDataTerm.key  = localizationDataTerm.key.Replace(".", "/");
        }

        _termsChache.Clear();

    }
}
