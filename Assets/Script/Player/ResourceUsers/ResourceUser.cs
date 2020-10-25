using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUser : MonoBehaviour
{
    public Sprite icon;
    public Item resource;
    public string userType;

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (Input.GetButtonDown("ItemAct") && HasEnoughResource())
        {
            bool succeed = ItemAct();
            if (succeed && Inventory.instance != null)
            {
                Inventory.instance.Remove(resource);
                AnimationManager.instance?.UseItem(userType);
            }
        }
    }

    protected bool HasEnoughResource()
    {
        if (Inventory.instance != null)
        {
            if(Inventory.instance.resourceCount.ContainsKey(resource))
            {
                if (Inventory.instance.resourceCount[resource] > 0)
                    return true;
            }
        }
        return false;
    }

    protected virtual bool ItemAct()
    {
        Debug.Log("Item Action");
        return false;
    }
}
