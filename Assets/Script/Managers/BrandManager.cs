using Defective.JSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrandManager : Singleton<BrandManager>
{
    public List<Brand> brands = new List<Brand>();
    //private ConfigFile _configFile = new ConfigFile();

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
            int id = -1;
            jBrand.GetField(ref id, "id");
            brands.Add(new Brand(id, brandName));
        }

        Resort();
    }

    void Resort()
    {
        brands = brands.OrderBy(o => o.brandName).ToList();
        brands = brands.OrderByDescending(o => o.favorite).ToList();
    }

    public void Save()
    {
        /*foreach (var brand in brands)
        {
            _configFile.SetValue("User", brand.brandName,brand.favorite);
        }

        _configFile.Save("user://BrandData.cfg");*/
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
    public int brandId = -1;
    public string brandName;
    public bool favorite = false;

    public Brand(string  brandName)
    {
        this.brandName = brandName;
    }

    public Brand(int id, string brandName)
    {
        this.brandId = id;
        this.brandName = brandName;
    }
}