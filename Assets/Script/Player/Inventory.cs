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
    Dictionary<ItemData, int> resourceCount = new Dictionary<ItemData, int>();
    [HideInInspector]
    List<ItemData> clues = new List<ItemData>();
    [HideInInspector]
    List<ItemData> gears = new List<ItemData>();

    public delegate void OnClueChanged();
    public OnClueChanged onClueChangedCallBack;

    public delegate void OnResourceChanged(Item resource);
    public OnResourceChanged onResourceChangedCallBack;

    public delegate void OnGearChanged(Item resource);
    public OnGearChanged onGearChangedCallBack;

    public delegate void ShowCluePanelCall(Item resource);
    public ShowCluePanelCall showCluePanelCall;

    public void Add(Item item)
    {
        Debug.Log("Add " + item.itemData.name);
        if(item.itemData.isResource)
        {
            if (resourceCount.ContainsKey(item.itemData))
            {
                Debug.Log("Get Resource " + item.itemData.name + " " + resourceCount[item.itemData]);
                resourceCount[item.itemData]++;
                onResourceChangedCallBack?.Invoke(item);
            }
            else 
            {
                Debug.Log("New Resource " + item.itemData.name);
                resourceCount.Add(item.itemData, 1);
                if (!gears.Contains(item.itemData))
                {
                    gears.Add(item.itemData);
                    onGearChangedCallBack?.Invoke(item);
                    showCluePanelCall?.Invoke(item);
                }
                onResourceChangedCallBack?.Invoke(item);
            }
        }
        else
        {
            Debug.Log("New Clue " + item.itemData.name);
            clues.Add(item.itemData);
            onClueChangedCallBack?.Invoke();
            showCluePanelCall?.Invoke(item);
        }
    }

    public void Remove(Item item)
    {
        if (item.itemData.isResource && resourceCount.ContainsKey(item.itemData))
        {
            resourceCount[item.itemData]--;
        }
        onResourceChangedCallBack?.Invoke(item);
    }

    public bool ContainClue(Item item)
    {
        return clues.Contains(item.itemData);
    }

    public bool ContainResource(Item item)
    {
        if (resourceCount.ContainsKey(item.itemData))
        {
            return resourceCount[item.itemData] > 0;
        }
        else
            return false;
    }

    public int GetResourceCount(Item item)
    {
        if (resourceCount.ContainsKey(item.itemData))
        {
            return resourceCount[item.itemData];
        }
        else
            return 0;
    }

    // deep copy
    public Dictionary<ItemData, int> GetResourceCount()
    {
        Dictionary<ItemData, int> newResourceCount = new Dictionary<ItemData, int>(resourceCount);
        return newResourceCount;
    }

    public List<ItemData> GetCluesList()
    {
        List<ItemData> newClues = new List<ItemData>(clues);
        return newClues;
    }

    public List<ItemData> GetGearsList()
    {
        List<ItemData> newGears = new List<ItemData>(gears);
        return newGears;
    }

    public void ResetData(Dictionary<ItemData, int> newResourceCount, List<ItemData> newClues)
    {
        resourceCount = newResourceCount;
        clues = newClues;

        onResourceChangedCallBack?.Invoke(null);
        onClueChangedCallBack?.Invoke();
    }
}
