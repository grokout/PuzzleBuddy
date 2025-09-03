using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static EventMsgManager;
using static System.Net.WebRequestMethods;

public class BarcodeManager : MonoBehaviour
{
    public void LookUp(string barcode)
    {
        StartCoroutine(LookUpI(barcode));
    }

    IEnumerator LookUpI(string barcode)
    { 
        string uri = "https://api.upcitemdb.com/prod/trial/lookup?upc=" + barcode;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    BarCodeScannedArgs barCodeScannedArgs = new BarCodeScannedArgs();
                    barCodeScannedArgs.scanData = webRequest.downloadHandler.text;
                    EventMsgManager.instance.SendEvent(GameEventIDs.BarcodeScanned, barCodeScannedArgs);
                    break;
            }
        }
    }
}
