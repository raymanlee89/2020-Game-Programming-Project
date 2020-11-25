using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

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
    Queue<Item> clueWaitingQueue = new Queue<Item>();

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
    public Text chapterText;
    public Text chapterMark;

    // loss panel
    public GameObject lossPanel;

    // loading panel
    public GameObject loadingPanel;
    public Light2D skyLight;
    public Text shortHintText;
    [TextArea(1, 10)]
    public string[] shortHints;
    int shortHintsIndex = 0;

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

        List<ItemData> clues = inventory.GetCluesList();
        for (int i = 0 ; i < clueSlots.Length ; i++)
        {
            if (i < clues.Count)
            {
                Item item = ScriptableObject.CreateInstance<Item>();
                item.itemData = clues[i];
                clueSlots[i].AddItem(item);
            }
            else
                clueSlots[i].ClearSlot();
        }
    }

    void UpdateGearList(Item resource)
    {
        Debug.Log("Update Gear List");

        List<ItemData> gears = inventory.GetGearsList();
        for (int i = 0; i < gearSlots.Length; i++)
        {
            if (i < gears.Count)
            {
                Item item = ScriptableObject.CreateInstance<Item>();
                item.itemData = gears[i];
                gearSlots[i].AddItem(item);
            }
            else
                gearSlots[i].ClearSlot();
        }
    }

    public void OpenBackpackPanel()
    {
        backpackPanel.SetActive(true);

        // stop time
        SoundManager.instance?.PauseAllSound();
        Heartbeat heartBeats = FindObjectOfType<Heartbeat>();
        heartBeats?.Pause();
        SoundManager.instance?.Play("OpenBackpack");
        Time.timeScale = 0f;
    }

    public void CloseBackpackPanel()
    {
        if (cluePanel.activeSelf)
        {
            clueWaitingQueue.Clear();
            CloseCluePanel();
        }

        backpackPanel.SetActive(false);
        
        // restart time
        SoundManager.instance?.UnPauseAllSound();
        Heartbeat heartBeats = FindObjectOfType<Heartbeat>();
        heartBeats?.UnPause();
        SoundManager.instance?.Play("CloseBackpack");
        Time.timeScale = 1f;
    }

    public void OpenCluePanel(Item item)
    {
        if(!backpackPanel.activeSelf)
            OpenBackpackPanel();
        if(!cluePanel.activeSelf)
        {
            cluePanel.SetActive(true);
            itemName.text = item.itemData.name;
            itemImage.sprite = item.itemData.image;
            itemDiscription.text = item.itemData.discription;
        }
        else
            clueWaitingQueue.Enqueue(item);
    }

    public void CloseCluePanel()
    {
        if(clueWaitingQueue.Count == 0)
            cluePanel.SetActive(false);
        else
        {
            Item item = clueWaitingQueue.Dequeue();
            cluePanel.SetActive(true);
            itemName.text = item.itemData.name;
            itemImage.sprite = item.itemData.image;
            itemDiscription.text = item.itemData.discription;
        }
    }

    public void OpenFrashlightUI(Item battery)
    {
        frashlightUI.SetActive(true);
        if(inventory.ContainResource(battery))
            batteryCount.text = "× " + inventory.GetResourceCount(battery).ToString("0");
        else
            batteryCount.text = "× 0";
        batteryIcon.sprite = battery.itemData.icon;
    }

    public void UpdatePowerBarNumber(string powerPercentage)
    {
        if (frashlightUI.activeSelf)
            powerBarNumber.text = powerPercentage + "%";
    }

    public void UpdateBatteryCount(Item battery)
    {
        if(frashlightUI.activeSelf)
            batteryCount.text = "× " + inventory.GetResourceCount(battery).ToString("0");
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

        resourceIcon.sprite = selectedUser.resource.itemData.icon;

        if (inventory.ContainResource(selectedUser.resource))
            resourceCount.text = "× " + inventory.GetResourceCount(selectedUser.resource).ToString("0");
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

    public void UpdateChapterText(string chapter)
    {
        chapterText.text = chapter;
        chapterMark.text = chapter;
        StartCoroutine(ShowChapterMark(5));
    }

    IEnumerator ShowChapterMark(float duration)
    {
        while(chapterMark.color.a < 1)
        {
            float newA = chapterMark.color.a + Time.deltaTime / duration;
            chapterMark.color = new Color(chapterMark.color.r, chapterMark.color.g, chapterMark.color.b,  newA);
            yield return null;
        }
        chapterMark.color = new Color(chapterMark.color.r, chapterMark.color.g, chapterMark.color.b, 1);
        while (chapterMark.color.a > 0)
        {
            float newA = chapterMark.color.a - Time.deltaTime / duration;
            chapterMark.color = new Color(chapterMark.color.r, chapterMark.color.g, chapterMark.color.b, newA);
            yield return null;
        }
        chapterMark.color = new Color(chapterMark.color.r, chapterMark.color.g, chapterMark.color.b, 0);
    }

    public void OpenLossPanel()
    {
        skyLight.intensity = 0;
        // stop time
        SoundManager.instance?.PauseAllSound();
        Heartbeat heartBeats = FindObjectOfType<Heartbeat>();
        heartBeats?.Pause();
        Time.timeScale = 0f;
        lossPanel.SetActive(true);
    }

    public void ClossLossPanel()
    {
        lossPanel.SetActive(false);
    }

    public void OpenLoadingPanel()
    {
        // restart time
        SoundManager.instance?.UnPauseAllSound();
        Heartbeat heartBeats = FindObjectOfType<Heartbeat>();
        heartBeats?.UnPause();
        Time.timeScale = 1f;

        loadingPanel.SetActive(true);
        StartCoroutine(ShowShortHints());
    }

    public void ClossLoadingPanel()
    {
        skyLight.intensity = 0.25f;
        loadingPanel.SetActive(false);
    }

    IEnumerator ShowShortHints()
    {
        while(loadingPanel.activeSelf)
        {
            shortHintText.text = shortHints[shortHintsIndex];
            shortHintsIndex = (shortHintsIndex + 1) % shortHints.Length;
            yield return new WaitForSeconds(3f);
        }
    }
}
