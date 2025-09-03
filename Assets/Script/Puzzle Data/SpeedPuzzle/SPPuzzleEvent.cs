
using static System.Net.Mime.MediaTypeNames;
using System.IO;



public class SPPuzzleEvent : PBPuzzle
{
    public string title = "";
    public string subTitle = "";
    public int number = 0;
    public string keyName;

    public override void AddTime(PBEntry entry)
    {
        // if dnf and no time entered
       /* if (entry.dnf)
        {
            entry.SetTime(GetMaxTime());
        }


        entry.pBPuzzle = this;
        entries.Add(entry);*/
    }

    /*public override bool ShowFilters()
    {
        return false;
    }

    public override string GetKey()
    {
        return Path.GetFileNameWithoutExtension(keyName);
    }

    public override void Save()
    {
        _configFile = new ConfigFile();
        base.Save();        

        _configFile.SetValue("Entrys", "title", title);
        _configFile.SetValue("Entrys", "subTitle", subTitle);
        _configFile.SetValue("Entrys", "number", number);
        _configFile.SetValue("Entrys", "keyName", keyName);

        _configFile.Save("user://Puzzle_" + GetKey() + ".cfg");
    }

    public override void LoadAll()
    {
        if (_allLoaded)
        {
            return;
        }

        Error err = _configFile.Load("user://Puzzle_" + GetKey() + ".cfg");

        title = (string)_configFile.GetValue("Entrys", "title");
        subTitle = (string)_configFile.GetValue("Entrys", "subTitle");
        number = (int)_configFile.GetValue("Entrys", "number");
        keyName = (string)_configFile.GetValue("Entrys", "keyName");

        base.LoadAll();
    }

    protected override void LoadEntry(JSONNode jEntry)
    {
        SPEntry entry = new SPEntry();
        entry.Load(jEntry);
        entry.pBPuzzle = this;
        entries.Add(entry);
    }*/
}

