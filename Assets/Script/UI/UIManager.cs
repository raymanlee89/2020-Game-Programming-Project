using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform itemParent;
    public GameObject backpackPanel;
    public GameObject cluePanel;
    public Image itemImage;
    public Text itemName;
    public Text itemDiscription;
    public Bar powerBar;
    public Bar healthBar;
    public Bar energyBar;

    ItemSlot[] slots;
    Inventory inventory;

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
        inventory.onClueChangedCallBack += UpdateBackpack;
        inventory.showCluePanelCall += ShowCluePanel;
        slots = itemParent.GetComponentsInChildren<ItemSlot>();
        backpackPanel.SetActive(false);
        cluePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Backpack"))
        {
            if (!backpackPanel.activeSelf && !cluePanel.activeSelf)
                ShowBackpackPanel();
            else
                CloseBackpackPanel();
        }
    }

    void UpdateBackpack()
    {
        Debug.Log("Update Backpack Panel");
        for(int i = 0 ; i < slots.Length ; i++)
        {
            if(i < inventory.clues.Count)
                slots[i].AddItem(inventory.clues[i]);
            else
                slots[i].ClearSlot();
        }
    }

    public void ShowBackpackPanel()
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

    public void ShowCluePanel(Item item)
    {
        if(!backpackPanel.activeSelf)
            ShowBackpackPanel();
        cluePanel.SetActive(true);
        itemName.text = item.name;
        itemImage.sprite = item.image;
        itemDiscription.text = item.discription;
    }

    public void CloseCluePanel()
    {
        cluePanel.SetActive(false);
    }
}
