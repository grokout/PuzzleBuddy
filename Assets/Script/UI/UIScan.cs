using BarcodeScanner;
using BarcodeScanner.Scanner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScan : UIBasePanel
{
    
    public RawImage imageWebCam;
    public AudioSource audioFound;
    public BarcodeManager barcodeManager;
    public Button buttonBack;

    private IScanner _barcodeScanner;
    private float _restartTime;

    void Awake()
    {
        //Screen.autorotateToPortrait = false;
        //Screen.autorotateToPortraitUpsideDown = false;
    }


    void Start()
    {
        buttonBack.onClick.AddListener(() =>
        {
            UIManager.instance.HidePanel("UIScan");
        });

        // Create a basic scanner
        _barcodeScanner = new Scanner();
        _barcodeScanner.Camera.Play();

        // Display the camera texture through a RawImage
        _barcodeScanner.OnReady += (sender, arg) => 
        {
            // Set Orientation & Texture
            imageWebCam.transform.localEulerAngles = _barcodeScanner.Camera.GetEulerAngles();
            imageWebCam.transform.localScale = _barcodeScanner.Camera.GetScale();
            imageWebCam.texture = _barcodeScanner.Camera.Texture;

            // Keep Image Aspect Ratio
            var rect = imageWebCam.GetComponent<RectTransform>();
            var newHeight = rect.sizeDelta.x * _barcodeScanner.Camera.Height / _barcodeScanner.Camera.Width;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

            _restartTime = Time.realtimeSinceStartup;


        };
    }


    public override void Show(PanelData panelData = null)
    {
        base.Show();
        if (_barcodeScanner != null)
        {
            _barcodeScanner.Camera.Play();
        }
    }

    public override void Hide()
    {
        base.Hide();
        _barcodeScanner.Stop();
    }

    private void StartScanner()
    {
        _barcodeScanner.Scan((barCodeType, barCodeValue) => 
        {
            _barcodeScanner.Stop();
            _restartTime += Time.realtimeSinceStartup + 1f;
            // Feedback
            audioFound.Play();

#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif

            Debug.Log("Found " + barCodeValue);
            barcodeManager.LookUp(barCodeValue);
            UIManager.instance.HidePanel("UIScan");
        });
    }

    void Update()
    {
        if (_barcodeScanner != null)
        {
            _barcodeScanner.Update();
        }

        // Check if the Scanner need to be started or restarted
        if (_restartTime != 0 && _restartTime < Time.realtimeSinceStartup)
        {
            StartScanner();
            _restartTime = 0;
        }
    }
}
