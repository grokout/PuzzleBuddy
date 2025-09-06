
public class TeamMembersManager : Singleton<TeamMembersManager>
{
    /*private ConfigFile _configFile = new ConfigFile();


    public List<TeamMember> teamMembers = new List<TeamMember> ();

    private int _nextId = 1;

    public void Load()
    {
        Error err = _configFile.Load("user://TeamMembersData.cfg");

        // If the file didn't load, ignore it.
        if (err != Error.Ok)
        {
            return;
        }

        _nextId = (int)_configFile.GetValue("User", "NextID");
        string jSonStr = (string)_configFile.GetValue("User", "TeamMembers");
        JSONNode jRoot = JSONObject.Parse(jSonStr);
        if (jRoot != null && jRoot.Count > 0)        
        {            
            for (int i = 0; i < jRoot.Count; i++)
            {
                JSONNode jTeamMember = jRoot[i];
                TeamMember member = new TeamMember();
                member.Deserialize(jTeamMember);
                teamMembers.Add(member);
            }
        }
    }

    public void Save()
    {
        JSONArray jArray = new JSONArray();

        foreach (TeamMember member in this.teamMembers)
        {
            JSONObject jTeamMember = member.Serialize();
            jArray.Add(jTeamMember);
        }

        _configFile.SetValue("User", "NextID", _nextId);
        _configFile.SetValue("User", "TeamMembers", jArray.ToString());

        _configFile.Save("user://TeamMembersData.cfg");
    }

    public void AddTeamMember(string teamMemberName)
    {
        TeamMember member = new TeamMember()
        { 
            name = teamMemberName 
        };
        member.SetId(_nextId++);
        teamMembers.Add(member);
        Save();
    }

    public void AddTeamMember(TeamMember teamMember)
    {
        teamMember.SetId(_nextId++);
        teamMembers.Add(teamMember);
        Save();
    }

    public void AddTeamMember(FriendData friendData)
    {
        TeamMember teamMember = new TeamMember()
        {
            name = friendData.GetDisplayName(),
            friendId = friendData.id
        };
        AddTeamMember(teamMember);
        GD.Print("Adding online Friend " + friendData.GetDisplayName());
    }

    public List<TeamMember> GetTeamMembersSorted()
    {
        List<TeamMember> list = teamMembers.OrderBy(o => o.name).ToList();

        return list;
    }

    public TeamMember GetTeamMember(int id)
    {
        foreach (TeamMember teamMember in teamMembers)
        {
            if (teamMember.id == id)
            {
                return teamMember;
            }
        }
        return null;
    }

    public TeamMember GetTeamMember(string friendId)
    {
        foreach (TeamMember teamMember in teamMembers)
        {
            if (teamMember.friendId == friendId)
            {
                return teamMember;
            }
        }
        return null;
    }

    public void AddOnlineFriends()
    {
        // check if any online friends are in our list
        foreach ( FriendData friendData in OnlineManager.instance.onlineFriends.friends.Values)
        {
            TeamMember teamMember = GetTeamMember(friendData.id);
            if (teamMember == null)
            {
                AddTeamMember(friendData);
            }
            else
            {
                // update display name                
                teamMember.name = friendData.GetDisplayName();
            }
        }
        Save();
    }*/
}
    