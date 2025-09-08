using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBrandSelect : UIBasePanel
{
    public UIListController listBrands;
    public Button buttonBack;
    public Button buttonEdit;


    void Start()
    {
        buttonBack.onClick.AddListener(() => UIManager.instance.HidePanel("UISelectBrand"));
        buttonEdit.onClick.AddListener(() =>
        {
            EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.TextEntered, OnTextEntered);
            UIManager.instance.ShowPanel("UIEnterText", new UIEnterTextData("Enter Brand Name"));
        });        
    }


    public override void Show(PanelData panelData = null)
    {
        base.Show();
        
        DisplayLists(); 
    }

    public override void Hide()
    {
        base.Hide();
        EventMsgManager.instance.RemoveListener(EventMsgManager.GameEventIDs.TextEntered, OnTextEntered);
    }

    void DisplayLists()
    {
        listBrands.ClearAll();


        List<Brand> brands = BrandManager.instance.brands;


        // sort?

        foreach (Brand brand in brands)
        {
            UIBrandMarker uIBrandMarker = listBrands.CreateMarker<UIBrandMarker>();
            uIBrandMarker.Set(brand);
        }

        listBrands.ResizeContainer();
    }

    void OnTextEntered(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.TextEnteredArgs textEnteredArgs = (EventMsgManager.TextEnteredArgs)args;


        Brand brand = BrandManager.instance.AddBrand(textEnteredArgs.textEntered);

        EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.BrandChanged, new EventMsgManager.BrandArgs(brand));
        UIManager.instance.HidePanel("UISelectBrand");
    }
}
