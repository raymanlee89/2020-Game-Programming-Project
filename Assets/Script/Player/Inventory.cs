using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one of instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    [HideInInspector]
    public Dictionary<Item, int> resourceCount = new Dictionary<Item, int>();
    [HideInInspector]
    public List<Item> clues;

    public GameObject cluePanel;
    public Image itemImage;
    public Text itemName;
    public Text itemDiscription;
    public GameObject backpackUI;

    public delegate void OnClueChanged();
    public OnClueChanged onClueChangedCallBack;

    public delegate void OnResourceChanged(Item resource);
    public OnResourceChanged onResourceChangedCallBack;

    private void Start()
    {
        cluePanel.SetActive(false);
    }

    public void Add(Item item)
    {
        if(item.isResource)
        {
            if (resourceCount.ContainsKey(item))
            {
                resourceCount[item]++;
                Debug.Log("Get Resource " + item.name + " " + resourceCount[item]);
            }
            else
            {
                resourceCount.Add(item, 1);
                Debug.Log("New Resource " + item.name + " " + resourceCount[item]);
            }
            onResourceChangedCallBack?.Invoke(item);
        }
        else
        {
            Debug.Log("New Clue " + item.name);
            clues.Add(item);
            onClueChangedCallBack?.Invoke();
            StartCoroutine(WaitAndShowCluePanel(item));
        }
    }

    public void Remove(Item item)
    {
        if (item.isResource && resourceCount.ContainsKey(item))
        {
            resourceCount[item]--;
        }
        onResourceChangedCallBack?.Invoke(item);
    }

    IEnumerator WaitAndShowCluePanel(Item item)
    {
        yield return new WaitForSeconds(0.2f);
        ShowCluePanel(item);
    }

    public void ShowCluePanel(Item item)
    {
        SoundManager.instance?.PauseAllSound();
        SoundSpeed heartBeats = FindObjectOfType<SoundSpeed>();
        heartBeats?.Pause();
        Time.timeScale = 0f;

        cluePanel.SetActive(true);
        itemName.text = item.name;
        itemImage.sprite = item.image;
        itemDiscription.text = item.discription;
    }

    public void CloseCluePanel()
    {
        if(!backpackUI.activeSelf)
        {
            SoundManager.instance?.UnPauseAllSound();
            SoundSpeed heartBeats = FindObjectOfType<SoundSpeed>();
            heartBeats?.UnPause();
            Time.timeScale = 1f;
        }
        cluePanel.SetActive(false);
    }
}
