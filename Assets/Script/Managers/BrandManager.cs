using Defective.JSON;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BrandManager : Singleton<BrandManager>
{
    public List<Brand> brands = new List<Brand>();
    //private ConfigFile _configFile = new ConfigFile();

    private List<Brand> _localBrands = new List<Brand>();

    public void LoadBrands()
    {
        TextAsset myTextFile = Resources.Load<TextAsset>("Brands");

        if (myTextFile != null)
        {
            // Access the text content
            string fileContent = myTextFile.text;
            Debug.Log("File content: " + fileContent);      

            JSONObject node =  new JSONObject(fileContent);

            JSONObject jRoot = node[0];
            foreach (JSONObject jBrand in jRoot.list) 
            {
                string brandName = jBrand.GetField("BrandResource").GetField("BrandName").stringValue;
                brands.Add(new Brand(brandName));
                Debug.Log("Add Brand: " + brandName);
            }
       
        }
        else
        {
            Debug.LogError("Text file not found in Resources folder.");
        }

        // Get locally saved brands
        string localBrands = PlayerPrefs.GetString("Brands");

        if (!string.IsNullOrEmpty(localBrands))
        {
            JSONObject jBrands = new JSONObject(localBrands);
            foreach (JSONObject jBrand in jBrands.list)
            {
                Brand brand = new Brand(jBrand);
                _localBrands.Add(brand);
                brands.Add(brand);
            }
        }

        Resort();

        SuprebaseOnline.instance.UpdateOnlineBrandList();

        LoadFavs();
    }

    public void OnRequestCompletedBrandList(string body)
    {
        Debug.Log("OnRequestCompleted Brand");
        string msg = body;
        JSONObject json =  new JSONObject(msg);

        foreach (JSONObject jBrand in json.list) 
        {
            string brandName = "";
            jBrand.GetField(ref brandName, "brand");
            brands.Add(new Brand(brandName));
        }

        Resort();
    }

    void Resort()
    {
        brands = brands.OrderBy(o => o.brandName).ToList();
        brands = brands.OrderByDescending(o => o.favorite).ToList();
    }

    public Brand AddBrand(string brandName)
    {
        Brand brand = new Brand(brandName);
        _localBrands.Add(brand);
        brands.Add(brand);  
        Save();
        return brand;
    }

    public void Save()
    {
        JSONObject jBrands = new JSONObject(JSONObject.Type.Array);
        foreach (Brand brand in _localBrands)
        {
            jBrands.Add(brand.Serialize());
        }

        PlayerPrefs.SetString("Brands",jBrands.ToString()); 
    }

    public void LoadFavs()
    {
       /* Error err = _configFile.Load("user://BrandData.cfg");

        // If the file didn't load, ignore it.
        if (err != Error.Ok)
        {
            return;
        }

        foreach (var brand in brands)
        {
            brand.favorite = (bool)_configFile.GetValue("User", brand.brandName, false);
        }

        Resort();*/
    }
}


public class Brand
{
    public string brandName;
    public bool favorite = false;

    public Brand(string  brandName)
    {
        this.brandName = brandName;
    }

    public Brand(JSONObject jSONObject)
    {
        Load(jSONObject);
    }

    public JSONObject Serialize()
    {
        JSONObject jSONObject = new JSONObject();
        jSONObject.SetField("brandName", brandName);
        jSONObject.SetField("favorite", favorite);
        return jSONObject;
    }

    public void Load(JSONObject jSONObject)
    {
        jSONObject.GetField(ref brandName, "brandName");
        jSONObject.GetField(ref favorite, "favorite");
    }
}