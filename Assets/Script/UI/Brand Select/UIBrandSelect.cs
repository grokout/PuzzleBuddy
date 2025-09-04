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
    }

    public override void Show()
    {
        base.Show();
        
        DisplayLists(); 
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
}
