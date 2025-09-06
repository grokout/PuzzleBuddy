using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIBrandMarker : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public Button Button;

    private Brand _brand;

    void Start()
    {
        Button.onClick.AddListener(() =>
        {
            EventMsgManager.instance.SendEvent(EventMsgManager.GameEventIDs.BrandChanged, new EventMsgManager.BrandArgs(_brand));
            UIManager.instance.HidePanel("UISelectBrand");
        });
    }

    public void Set(Brand brand)
    {
        _brand = brand;
        textTitle.text = brand.brandName;
    }



}

