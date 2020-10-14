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
        if(Input.GetButtonDown("OpenBackpack"))
        {
            SoundManager.instance?.Play("OpenBackpack");
            backpackUI.SetActive(!backpackUI.activeSelf);
            if (backpackUI.activeSelf)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
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
