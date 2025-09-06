using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Dates;
using System;
using Defective.JSON;
using ZXing.Common;
using static UnityEngine.EventSystems.EventTrigger;
public class UIAdd : UIBasePanel
{
    public TMP_InputField inputPuzzleName;
    public TMP_InputField inputPuzzleBrand;
    public TMP_InputField inputPuzzlePieceCount;
    public TMP_InputField inputTimeHours;
    public TMP_InputField inputTimeMinutes;
    public TMP_InputField inputTimeSeconds;
    public TMP_InputField inputBarcode;
    public Button buttonSave;
    public Button buttonBack;
    public Button buttonScan;
    public Button buttonBrandSelect;
    public Button buttonPieceCountSelect;
    public DatePicker datePicker;
    public TMP_Dropdown dropdownTeam;
    public UITeamMemberSelector teamMemberSelector1;
    public UITeamMemberSelector teamMemberSelector2;
    public UITeamMemberSelector teamMemberSelector3;

    void Start()
    {
        buttonBack.onClick.AddListener(() =>
        {
            UIManager.instance.HidePanel("UIAdd");
            UIManager.instance.ShowPanel("UIHome");
            //UIManager.instance.ShowPanel("UIViewResults");
        });

        buttonScan.onClick.AddListener(() =>
        {
            UIManager.instance.ShowPanel("UIScan");
        });

        dropdownTeam.onValueChanged.AddListener((value) =>
        {
            OnSelectTeamSize(value);
        });

        inputPuzzleName.onValueChanged.AddListener((value) => 
        {
            buttonSave.interactable = !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(inputPuzzleBrand.text);
        });

        inputPuzzleName.onValueChanged.AddListener((value) =>
        {
            buttonSave.interactable = !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(inputPuzzleName.text);
        });

        buttonBrandSelect.onClick.AddListener(() => UIManager.instance.ShowPanel("UISelectBrand"));
        buttonPieceCountSelect.onClick.AddListener(() => UIManager.instance.ShowPanel("UISelectPieceCount"));

        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.BarcodeScanned, OnBarCodeScanned);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.CountChanged, OnCountChanged);
        EventMsgManager.instance.AddListener(EventMsgManager.GameEventIDs.BrandChanged, OnBrandChanged);

        buttonSave.onClick.AddListener(() =>
        {
            

            // TODO Check if we had an entry for edit
            PBEntry pBEntry = new PBEntry()
            {
                userId = OnlineManager.instance.GetUserId(),
                puzzleName = inputPuzzleName.text,
                brand = inputPuzzleBrand.text,

            };
            int.TryParse(inputBarcode.text, out pBEntry.puzzleUpc);
            int.TryParse(inputPuzzlePieceCount.text, out pBEntry.puzzleCount);

            int.TryParse(inputTimeHours.text, out int h);
            int.TryParse(inputTimeMinutes.text, out int m);
            int.TryParse(inputTimeSeconds.text, out int s);
            float timeInMins = (h * 60) + m;
            if (s != 0)
            {
                timeInMins += (s / 60f);
            }

            pBEntry.dnf = timeInMins == 0;
            
            pBEntry.SetTime(timeInMins);

            pBEntry.date = datePicker.SelectedDate;

            List<string> teamMembers = new List<string>();
            if (dropdownTeam.value > 0)
            {
                teamMembers.Add(teamMemberSelector1.text);
            }
            if (dropdownTeam.value > 2)
            {
                teamMembers.Add(teamMemberSelector2.text);
                teamMembers.Add(teamMemberSelector3.text);
            }

            pBEntry.SetTeamMembers(teamMembers);

            PBPuzzleManager.instance.AddTime(pBEntry);
            SuprebaseOnline.instance.AddEntry(pBEntry); 

            UIManager.instance.ShowPanel("UIViewResults");
            UIManager.instance.ShowPanel("UIHome");
            UIManager.instance.HidePanel("UIAdd");

        });
    }


    public override void Show(PanelData panelData = null)
    {
        base.Show(); 
        OnSelectTeamSize(0);
        datePicker.SelectedDate = DateTime.Now;
        buttonSave.interactable = false;

        inputPuzzleName.text = "";
        inputPuzzleBrand.text = "";
        inputPuzzlePieceCount.text = "";
        inputTimeHours.text = "";
        inputTimeMinutes.text = "";
        inputTimeSeconds.text = "";
        inputBarcode.text = "";
    }

    private void OnSelectTeamSize(int index)
    {
        int numMembers = 0;
        switch (index)
        {
            case 1:
                numMembers = 1;
                break;
            case 2:
                numMembers = 3;
                break;
        }

        teamMemberSelector1.gameObject.SetActive(numMembers > 0);
        teamMemberSelector2.gameObject.SetActive(numMembers > 2);
        teamMemberSelector3.gameObject.SetActive(numMembers > 2);

    }


    void OnBarCodeScanned(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.BarCodeScannedArgs barCodeScannedArgs = (EventMsgManager.BarCodeScannedArgs)args;


        JSONObject jSONObject = new JSONObject(barCodeScannedArgs.scanData);

        JSONObject jItems = jSONObject.GetField("items");
        if (jItems != null)
        {
            JSONObject jEntry = jItems.list[0];
            string title = "";
            jEntry.GetField(ref title, "title");

            int pc = StringUtils.GetNumberFromString(title);

            inputPuzzlePieceCount.text = pc.ToString();

            // Remove the number from the title

            if (jEntry.HasField("isbn"))
            { 
                inputBarcode.text = jEntry.GetField("isbn").stringValue;
            }
            else
            {
                inputBarcode.text = "";
            }    

            int s = title.IndexOf(pc.ToString());
            if (s < 10)
            {
                // remove from front
                title = title.Substring(s + pc.ToString().Length + 1);
            }

            inputPuzzleName.text = title;
        }
    }

    void OnBrandChanged(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.BrandArgs brandArgs = (EventMsgManager.BrandArgs)args;
        inputPuzzleBrand.text = brandArgs.brand.brandName;
    }

    void OnCountChanged(EventMsgManager.GameEventArgs args)
    {
        EventMsgManager.CountArgs countArgs = (EventMsgManager.CountArgs)args;  
        inputPuzzlePieceCount.text = countArgs.count.ToString();
    }    
}
