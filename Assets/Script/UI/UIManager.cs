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
    public GameObject stealBarPanel;
    [HideInInspector]
    public Bar stealBar;

    // clue panel
    public GameObject cluePanel;
    public Image itemImage;
    public GameObject itemDetail;
    public Text itemName;
    public Text itemDiscription;
    Queue<Item> clueWaitingQueue = new Queue<Item>();
    Item presentItem = null;

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
    public Text positionText;
    public GameObject mapPanel;

    // loss panel
    public GameObject lossPanel;

    // loading panel
    public GameObject loadingPanel;
    public Text shortHintText;
    [TextArea(1, 10)]
    public string[] shortHints;
    int shortHintsIndex = 0;
    public Bar loadingBar;

    // trophies
    public GameObject trophyPanel;
    public List<GameObject> trophies; // 0 : pass game, 1 : 0 respawn, 2 : 20 minutes, 3 : all clues, 4 : no gear, 5 : all trophies
    public Text respawnTimesText;
    public Text totalGamingTimeText;

    // video
    public GameObject videoScreen;
    float videoFadeDuration = 3;

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
        stealBar = stealBarPanel.GetComponent<Bar>();
        stealBarPanel.SetActive(false);
        frashlightUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Backpack") && !DialogueManager.instance.IsDialogueStarted())
        {
            if (!backpackPanel.activeSelf)
                OpenBackpackPanel();
            else
                CloseBackpackPanel();
        }

        if (Input.GetButtonDown("Map") && !DialogueManager.instance.IsDialogueStarted())
        {
            if (!mapPanel.activeSelf)
                OpenMapPanel();
            else
                CloseMapPanel();
        }

        if(Input.GetButtonDown("CluePanel") && cluePanel.activeSelf && !DialogueManager.instance.IsDialogueStarted())
        {
            CloseCluePanel();
        }

        // deal with stop time or player disable bug
        if (Input.GetButtonDown("RestartTime"))
        {
            GameManager.instance?.RestartTime();
        }
    }

    void UpdateClueList()
    {
        Debug.Log("Update Clue List");

        List<Item> clues = inventory.GetCluesList();
        for (int i = 0 ; i < clueSlots.Length ; i++)
        {
            if (i < clues.Count)
            {
                clueSlots[i].AddItem(clues[i]);
            }
            else
                clueSlots[i].ClearSlot();
        }
    }

    void UpdateGearList(Item resource)
    {
        Debug.Log("Update Gear List");

        List<Item> gears = inventory.GetGearsList();
        for (int i = 0; i < gearSlots.Length; i++)
        {
            if (i < gears.Count)
            {
                gearSlots[i].AddItem(gears[i]);
            }
            else
                gearSlots[i].ClearSlot();
        }
    }

    public void OpenBackpackPanel()
    {
        backpackPanel.SetActive(true);
        // stop time
        GameManager.instance.StopTime();
        SoundManager.instance?.Play("OpenBackpack");
    }

    public void CloseBackpackPanel()
    {
        Debug.Log("Close backpack");
        backpackPanel.SetActive(false);

        // restart time
        GameManager.instance.RestartTime();
        SoundManager.instance?.Play("CloseBackpack");
    }

    public void OpenMapPanel()
    {
        mapPanel.SetActive(true);
        // stop time
        // GameManager.instance?.StopTime();
        // SoundManager.instance?.Play("OpenBackpack");
    }

    public void CloseMapPanel()
    {
        mapPanel.SetActive(false);

        // restart time
        // GameManager.instance?.RestartTime();
        // SoundManager.instance?.Play("CloseBackpack");
    }

    public void OpenCluePanel(Item item)
    {
        Debug.Log("Open Clue Panel");
        if (!cluePanel.activeSelf)
        {
            presentItem = item;
            GameManager.instance.StopTime();
            cluePanel.SetActive(true);
            itemName.text = item.itemData.name;
            itemImage.sprite = item.itemData.image;
            itemDiscription.text = item.itemData.discription;
        }
        else if (!clueWaitingQueue.Contains(item) && item != presentItem)
        {
            clueWaitingQueue.Enqueue(item);
        }
    }

    public void CloseCluePanel()
    {
        Debug.Log("Close Clue Panel");
        if (clueWaitingQueue.Count == 0)
        {
            cluePanel.SetActive(false);
            itemDetail?.SetActive(false);
            GameManager.instance.RestartTime();
        }
        else
        {
            presentItem = clueWaitingQueue.Dequeue();
            itemName.text = presentItem.itemData.name;
            itemImage.sprite = presentItem.itemData.image;
            itemDiscription.text = presentItem.itemData.discription;
        }
    }

    public void ClueDetail()
    {
        if (!itemDetail.activeSelf)
        {
            itemDetail.GetComponent<Image>().sprite = itemImage.sprite;
            itemDetail.SetActive(true);
        }
        else
            itemDetail.SetActive(false);
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

    IEnumerator coroutine = null;
    public void UpdatePositionText(string position)
    {
        if(position != null)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
            positionText.text = position;
            coroutine = ShowPosition(2);
            StartCoroutine(coroutine);
        }
        else
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StopShowPosition(2);
            StartCoroutine(coroutine);
        }
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

    IEnumerator ShowPosition(float duration)
    {
        while (positionText.color.a < 1)
        {
            float newA = positionText.color.a + Time.deltaTime / duration;
            positionText.color = new Color(positionText.color.r, positionText.color.g, positionText.color.b, newA);
            yield return null;
        }
        positionText.color = new Color(positionText.color.r, positionText.color.g, positionText.color.b, 1);
    }

    IEnumerator StopShowPosition(float duration)
    {
        while (positionText.color.a > 0)
        {
            float newA = positionText.color.a - Time.deltaTime / duration;
            positionText.color = new Color(positionText.color.r, positionText.color.g, positionText.color.b, newA);
            yield return null;
        }
        positionText.color = new Color(positionText.color.r, positionText.color.g, positionText.color.b, 0);
        positionText.text = "";
    }

    public void OpenLossPanel()
    {
        lossPanel.SetActive(true);
    }

    public void CloseLossPanel()
    {
        lossPanel.SetActive(false);
    }

    public void OpenLoadingPanel()
    {
        loadingPanel.SetActive(true);
        loadingBar.SetMaxFill(8);
        loadingBar.SetFill(0);
        StartCoroutine(ShowShortHints());
    }

    public void CloseLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    IEnumerator ShowShortHints()
    {
        int progress = 0;
        while(loadingPanel.activeSelf)
        {
            progress += 1;
            loadingBar.SetFill(progress);
            shortHintText.text = shortHints[shortHintsIndex];
            shortHintsIndex = (shortHintsIndex + 1) % shortHints.Length;
            yield return new WaitForSeconds(1f);
            progress += 1;
            loadingBar.SetFill(progress);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ShowTrophy()
    {
        int respawnTimes = GameManager.instance.respawnTimes;
        float totalTime = GameManager.instance.GetGamingTime();
        int cluesCount = Inventory.instance.GetClueAmount();
        int gearCount = Inventory.instance.GetGearCount();

        bool[] trophiesIsOn = new bool[] { true, false, false, false, false, false };
        if (respawnTimes == 0)
            trophiesIsOn[1] = true;

        if (totalTime < 60 * 20)
            trophiesIsOn[2] = true;

        if (cluesCount == 24)
            trophiesIsOn[3] = true;

        if (gearCount <= 3)
            trophiesIsOn[4] = true;

        if (trophiesIsOn[1] && trophiesIsOn[2] && trophiesIsOn[3] && trophiesIsOn[4])
            trophiesIsOn[5] = true;

        trophyPanel.SetActive(true);
        for(int i=0 ; i< trophiesIsOn.Length; i++)
        {
            float newA = 0;
            if (trophiesIsOn[i])
                newA = 1;
            else
                newA = 0.3f;
            trophies[i].GetComponentInChildren<Image>().color = new Color(1, 1, 1, newA);
            foreach (Text text in trophies[i].GetComponentsInChildren<Text>())
                text.color = new Color(1, 1, 1, newA);
        }

        respawnTimesText.text = respawnTimes.ToString();
        int minutes = (int)(totalTime / 60);
        int seconds = (int)(totalTime % 60);
        totalGamingTimeText.text = minutes.ToString() + " min " + seconds.ToString() + " sec";
    }

    public void OpenVideoScreen()
    {
        StartCoroutine(FadeInVideoScreen());
    }

    public void CloseVideoScreen()
    {
        StartCoroutine(FadeOutVideoScreen());
    }

    IEnumerator FadeInVideoScreen()
    {
        videoScreen.SetActive(true);
        RawImage image = videoScreen.GetComponent<RawImage>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        float newA = 0;
        while (newA < 1)
        {
            newA += Time.deltaTime / videoFadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, newA);
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    IEnumerator FadeOutVideoScreen()
    {
        RawImage image = videoScreen.GetComponent<RawImage>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        float newA = 1;
        while (newA > 0)
        {
            newA -= Time.deltaTime / videoFadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, newA);
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        videoScreen.SetActive(false);
    }
}
