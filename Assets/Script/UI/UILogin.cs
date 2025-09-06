using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILogin : UIBasePanel
{
    public Button buttonLogin;
    public Button buttonRegister;
    //public Button buttonSkip;
    public TMP_InputField inputName;
    public TMP_InputField inputPassword;
    public Toggle toggleAutoLogin;
    public TextMeshProUGUI textMessage;
    public GameObject goMessagePnael;

    void Start()
    {

        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.LoginFailed, OnLoginFailed);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.RegisterFailed, OnRegisterFailed);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.LoginSuccess, OnLoginSuccess);

        toggleAutoLogin.onValueChanged.AddListener(OnAutoLoginPressed);

        buttonLogin.onClick.AddListener(LoginUser);
        buttonRegister.onClick.AddListener(RegisterUser);
        //buttonSkip.onClick.AddListener(OnSkip);
    }

    public override void Show(PanelData panelData = null)
    {
        base.Show();

        // If toggle remember
        inputName.text = UserManager.instance.GetLoginName();
        inputPassword.text = UserManager.instance.GetPassword();
        goMessagePnael.SetActive(false);


        toggleAutoLogin.isOn = UserManager.instance.CanAutoLogin();

        if (UserManager.instance.CanAutoLogin())
        {
            LoginUser();
        }
    }

    private void OnAutoLoginPressed(bool toggleOn)
    {
        UserManager.instance.SetAutoLogin(toggleOn);
        UserManager.instance.Save();
    }

    void GoToNext()
    {
        UIManager.instance.HidePanel("UILogin");
        UIManager.instance.ShowPanel("UIHome");
        OnlineManager.instance.OnLogin();
    }


    void LoginUser()
    {
        if (!VerifyEntries())
        {
            return;
        }

        ShowWorking();
        OnlineManager.instance.LoginUser(inputName.text, inputPassword.text);
    }

    void RegisterUser()
    {
        if (!VerifyEntries())
        {
            return;
        }
        ShowWorking();
        OnlineManager.instance.RegisterNewUser(inputName.text, inputPassword.text);
    }

    void OnSkip()
    {
        OnLoginSuccess(null);
        return;
    }
    void ShowWorking()
    {
        buttonLogin.interactable = false;
        buttonRegister.interactable = false;
    }

    void StopWorking()
    {
        buttonLogin.interactable = true;
        buttonRegister.interactable = true;
    }

    bool VerifyEntries()
    {
        if (inputName.text == "" || inputPassword.text == "")
        {
            // TODO show warning
            return false;
        }

        UserManager.instance.SetLoginCreds(inputName.text, inputPassword.text);

        return true;
    }

    public void OnLoginSuccess(EventMsgManager.GameEventArgs args)
    {
        StopWorking();
        GoToNext();
    }

    public void OnLoginFail(string message)
    {
        StopWorking();
        textMessage.text = message;
        goMessagePnael.SetActive(true);
    }

    public void OnRegisterFailed(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.ErrorArgs errorArgs = (EventMsgManager.ErrorArgs)args;
        OnLoginFail(errorArgs.msg);
    }

    public void OnLoginFailed(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.ErrorArgs errorArgs = (EventMsgManager.ErrorArgs)args;
        OnLoginFail(errorArgs.msg);
    }
}
