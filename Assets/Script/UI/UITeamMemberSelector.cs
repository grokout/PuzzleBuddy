using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITeamMemberSelector : MonoBehaviour
{
    public TMP_InputField inputName;
    public Button buttonTeamMemberQuickSelect;

    void Start()
    {
        
    }

    public string text
    {
        get { return inputName.text; }
    }
}
