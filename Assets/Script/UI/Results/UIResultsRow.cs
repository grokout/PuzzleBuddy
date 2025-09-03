using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResultsRow : MonoBehaviour
{
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textInfo;
    public TextMeshProUGUI textPPM;
    public Button buttonEdit;


    private PBEntry _entry;
    void Start()
    {

    }

    public void Set(PBEntry entry)
    {
        _entry = entry;
        textTime.text  = entry.GetTimeString();
        textInfo.text = entry.GetInfoText();
        textPPM.text = entry.GetPScore().ToString("N2");
    }

}
