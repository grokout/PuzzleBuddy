using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIEnterText : UIBasePanel
{
    public TMP_InputField inputText;
    public Button buttonBack;
    public TextMeshProUGUI textTitle;

    void Start()
    {
        buttonBack.onClick.AddListener(() => UIManager.instance.HidePanel("UIEnterText"));
        inputText.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show(panelData);

        inputText.text = "";
        textTitle.text = ((UIEnterTextData)panelData).title;
    }

    void OnInputFieldEndEdit(string text)
    {
        buttonBack.onClick.AddListener(() => UIManager.instance.HidePanel("UIEnterText"));
        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.TextEntered,new EventMsgManager.TextEnteredArgs(inputText.text));  
    }
}

public class UIEnterTextData : PanelData
{
    public string title;
   
    public UIEnterTextData(string title)
    {
        this.title = title;
    }   
}