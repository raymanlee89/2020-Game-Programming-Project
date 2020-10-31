using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // player bar
    public Bar healthBar;
    public Bar energyBar;

    // clue panel
    public GameObject cluePanel;
    public Image itemImage;
    public Text itemName;
    public Text itemDiscription;
   
    // backpack panel
    public GameObject backpackPanel;
    public Transform clueParent;
    public Transform gearParent;
    ItemSlot[] clueSlots;
    ItemSlot[] gearSlots;
    Inventory inventory;

    // frashlight UI
    public GameObject frashlightUI;
    public Bar powerBar;
    public Text powerBarNumber;
    public Image batteryIcon;
    public Text batteryCount;

    // resource user UI
    public GameObject resourceUserUI;
    public Image selectedUserIcon;
    public Image formerUserIcon;
    public Image laterUserIcon;
    public Image resourceIcon;
    public Text resourceCount;

    // hints
    public Text hintText;

    #region Singleton
    public static UIManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of UImanager found!");
            return;
        }
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onClueChangedCallBack += UpdateClueList;
        inventory.onResourceChangedCallBack += UpdateGearList;
        inventory.showCluePanelCall += OpenCluePanel;
        clueSlots = clueParent.GetComponentsInChildren<ItemSlot>();
        gearSlots = gearParent.GetComponentsInChildren<ItemSlot>();
        backpackPanel.SetActive(false);
        cluePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Backpack"))
        {
            if (!backpackPanel.activeSelf && !cluePanel.activeSelf)
                OpenBackpackPanel();
            else
                CloseBackpackPanel();
        }
    }

    void UpdateClueList()
    {
        Debug.Log("Update Clue List");
        for(int i = 0 ; i < clueSlots.Length ; i++)
        {
            if(i < inventory.clues.Count)
                clueSlots[i].AddItem(inventory.clues[i]);
            else
                clueSlots[i].ClearSlot();
        }
    }

    void UpdateGearList(Item resource)
    {
        Debug.Log("Update Gear List");
        for (int i = 0; i < gearSlots.Length; i++)
        {
            if (i < inventory.gears.Count)
                gearSlots[i].AddItem(inventory.gears[i]);
            else
                gearSlots[i].ClearSlot();
        }
    }

    public void OpenBackpackPanel()
    {
        backpackPanel.SetActive(true);

        // stop time
        SoundManager.instance?.PauseAllSound();
        SoundSpeed heartBeats = FindObjectOfType<SoundSpeed>();
        heartBeats?.Pause();
        SoundManager.instance?.Play("OpenBackpack");
        Time.timeScale = 0f;
    }

    public void CloseBackpackPanel()
    {
        backpackPanel.SetActive(false);
        
        if (cluePanel.activeSelf)
            CloseCluePanel();

        // restart time
        SoundManager.instance?.UnPauseAllSound();
        SoundSpeed heartBeats = FindObjectOfType<SoundSpeed>();
        heartBeats?.UnPause();
        SoundManager.instance?.Play("CloseBackpack");
        Time.timeScale = 1f;
    }

    public void OpenCluePanel(Item item)
    {
        if(!backpackPanel.activeSelf)
            OpenBackpackPanel();
        cluePanel.SetActive(true);
        itemName.text = item.name;
        itemImage.sprite = item.image;
        itemDiscription.text = item.discription;
    }

    public void CloseCluePanel()
    {
        cluePanel.SetActive(false);
    }

    public void OpenFrashlightUI(Item battery)
    {
        frashlightUI.SetActive(true);
        if(inventory.resourceCount.ContainsKey(battery))
            batteryCount.text = "× " + (inventory.resourceCount[battery]).ToString("0");
        else
            batteryCount.text = "× 0";
        batteryIcon.sprite = battery.icon;
    }

    public void UpdatePowerBarNumber(string powerPercentage)
    {
        if (frashlightUI.activeSelf)
            powerBarNumber.text = powerPercentage + "%";
    }

    public void UpdateBatteryCount(Item battery)
    {
        if(frashlightUI.activeSelf)
            batteryCount.text = "× " + (inventory.resourceCount[battery]).ToString("0");
    }

    public void CloseFrashlightUI()
    {
        frashlightUI?.SetActive(false);
    }

    public void OpenResourceUserUI()
    {
        resourceUserUI.SetActive(true);
    }

    public void UpdateResourceUserUI(ResourceUser selectedUser, ResourceUser formerUser, ResourceUser laterUser)
    {
        if (formerUser == null)
            formerUserIcon.enabled = false;
        else
        {
            formerUserIcon.enabled = true;
            formerUserIcon.sprite = formerUser.icon;
        }


        if (laterUser == null)
            laterUserIcon.enabled = false;
        else
        {
            laterUserIcon.enabled = true;
            laterUserIcon.sprite = laterUser.icon;
        }
            

        selectedUserIcon.sprite = selectedUser.icon;

        resourceIcon.sprite = selectedUser.resource.icon;

        if (inventory.resourceCount.ContainsKey(selectedUser.resource))
            resourceCount.text = "× " + (inventory.resourceCount[selectedUser.resource]).ToString("0");
        else
            resourceCount.text = "× 0";
    }

    public void CloseResourceUserUI()
    {
        resourceUserUI.SetActive(false);
    }

    public void UpdateHintText(string hint)
    {
        hintText.text = hint;
    }
}
