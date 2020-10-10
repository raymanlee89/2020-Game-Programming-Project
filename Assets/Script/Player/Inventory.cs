using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Dictionary<Item, int> resourceCount = new Dictionary<Item, int>();
    public List<Item> clues;

    public delegate void OnClueChanged();
    public OnClueChanged onClueChangedCallBack;

    public delegate void OnResourceChanged(Item resource);
    public OnResourceChanged onResourceChangedCallBack;

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
}
