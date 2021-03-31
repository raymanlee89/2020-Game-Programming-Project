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
    Dictionary<Item, int> resourceCount = new Dictionary<Item, int>();
    [HideInInspector]
    List<Item> clues = new List<Item>();
    [HideInInspector]
    List<Item> gears = new List<Item>();

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
            if (resourceCount.ContainsKey(item))
            {
                Debug.Log("Get Resource " + item.itemData.name + " " + resourceCount[item]);
                resourceCount[item]++;
                onResourceChangedCallBack?.Invoke(item);
            }
            else 
            {
                Debug.Log("New Resource " + item.itemData.name);
                resourceCount.Add(item, 1);
                if (!gears.Contains(item))
                {
                    gears.Add(item);
                    onGearChangedCallBack?.Invoke(item);
                    showCluePanelCall?.Invoke(item);
                }
                onResourceChangedCallBack?.Invoke(item);
            }
        }
        else
        {
            if(!clues.Contains(item))
            {
                Debug.Log("New Clue " + item.itemData.name);
                clues.Add(item);
                onClueChangedCallBack?.Invoke();
                showCluePanelCall?.Invoke(item);
            }
        }
    }

    public void Remove(Item item)
    {
        if (item.itemData.isResource && resourceCount.ContainsKey(item))
        {
            resourceCount[item]--;
        }
        onResourceChangedCallBack?.Invoke(item);
    }

    public bool ContainClue(Item item)
    {
        return clues.Contains(item);
    }

    public bool ContainResource(Item item)
    {
        if (resourceCount.ContainsKey(item))
        {
            return resourceCount[item] > 0;
        }
        else
            return false;
    }

    public int GetResourceCount(Item item)
    {
        if (resourceCount.ContainsKey(item))
        {
            return resourceCount[item];
        }
        else
            return 0;
    }

    public int GetClueAmount()
    {
        return clues.Count;
    }

    public int GetGearCount()
    {
        return gears.Count;
    }

    public Dictionary<Item, int> GetResourceCount()
    {
        return resourceCount;
    }

    public List<Item> GetCluesList()
    {
        return clues;
    }

    public List<Item> GetGearsList()
    {
        return gears;
    }

    public void ResetData(Dictionary<Item, int> newResourceCount)
    {
        resourceCount.Clear();
        resourceCount = new Dictionary<Item, int> (newResourceCount); // deep copy

        onResourceChangedCallBack?.Invoke(null);
    }
}
