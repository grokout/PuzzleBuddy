using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : UIBasePanel
{
    public Button buttonViewResults;
    public Button buttonAdd;
    public Button buttonFriends;
    public Button buttonAccount;


    void Start()
    {
        BrandManager.instance.LoadBrands();

        buttonViewResults.onClick.AddListener(() =>
        {
            UIManager.instance.HidePanel("UIAdd");
            UIManager.instance.ShowPanel("UIHome");
            UIManager.instance.ShowPanel("UIViewResults");

        });

        buttonAdd.onClick.AddListener(() =>
        {
            UIManager.instance.HidePanel("UIHome");
            UIManager.instance.HidePanel("UIViewResults");
            UIManager.instance.ShowPanel("UIAdd");
        });
    }


    public override void Show()
    {
        base.Show();
        UIManager.instance.ShowPanel("UIViewResults");
    }
}
