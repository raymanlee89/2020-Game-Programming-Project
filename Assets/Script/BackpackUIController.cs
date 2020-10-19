using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackUIController : MonoBehaviour
{
    public Transform itemParent;
    public GameObject backpackUI;
    ItemSlot[] slots;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onClueChangedCallBack += UpdateUI;
        slots = itemParent.GetComponentsInChildren<ItemSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Backpack"))
        {
            
            backpackUI.SetActive(!backpackUI.activeSelf);
            if (backpackUI.activeSelf)
            {
                SoundManager.instance?.PauseAllSound();
                SoundSpeed heartBeats = FindObjectOfType<SoundSpeed>();
                heartBeats?.Pause();
                SoundManager.instance?.Play("OpenBackpack");
                Time.timeScale = 0f;
            }
            else
            {
                SoundManager.instance?.UnPauseAllSound();
                SoundSpeed heartBeats = FindObjectOfType<SoundSpeed>();
                heartBeats?.UnPause();
                SoundManager.instance?.Play("CloseBackpack");
                Time.timeScale = 1f;
            } 
        }
    }

    void UpdateUI()
    {
        Debug.Log("Update UI");
        for(int i = 0 ; i < slots.Length ; i++)
        {
            if(i < inventory.clues.Count)
            {
                slots[i].AddItem(inventory.clues[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
