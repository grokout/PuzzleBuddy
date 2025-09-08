using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResultsRow : MonoBehaviour
{
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textInfo;
    public TextMeshProUGUI textInfo2;
    public TextMeshProUGUI textPPM;
    public Button buttonEdit;
    public Image imageBG;

    public Color colorUser;
    public Color colorFriend;

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

        if (buttonEdit != null)
        {
            buttonEdit.gameObject.SetActive(_entry.userId == OnlineManager.instance.GetUserId());
        }

        if (imageBG != null)
        {
            imageBG.color = _entry.userId == OnlineManager.instance.GetUserId() ? colorUser : colorFriend;
        }

        if (textInfo2 != null)
        {
            if (_entry.userId != OnlineManager.instance.GetUserId())
            {
                textInfo2.gameObject.SetActive(true);
                textInfo2.text = _entry.GetInfoText2();
            }
            else
            {
                textInfo2.gameObject.SetActive(false);
            }
        }
    }
}
