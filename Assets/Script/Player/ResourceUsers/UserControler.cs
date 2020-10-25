using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserControler : MonoBehaviour
{
    int totalUserCount = 0;
    public Image userIcon;
    public Image resourceIcon;
    public Text resourceCount;
    public Image batteryIcon;
    public Text batteryCount;
    public Sprite defaultImage;
    public Sprite defaultWithFrashlightImage;
    public Sprite changingImage;
    public Sprite changingWithFrashlightImage;
    public FrashlightUser frashlightUser;
    ResourceUser[] users = null;
    Inventory inventory;
    int selectedUser = 0;

    #region Singleton
    public static UserControler instance;

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
        inventory.onResourceChangedCallBack += ResourceCountChange;
        totalUserCount = SelectUser();
        frashlightUser = GetComponent<FrashlightUser>();
        batteryCount.text = "× 0";
        batteryIcon.sprite = frashlightUser.resource.icon;
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedUser = selectedUser;
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedUser++;
            SoundManager.instance?.Play("ChangeUser");
            if (selectedUser > totalUserCount - 1)
                selectedUser = 0;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedUser--;
            SoundManager.instance?.Play("ChangeUser");
            if (selectedUser < 0)
                selectedUser = totalUserCount - 1;
        }

        if(previousSelectedUser != selectedUser)
        {
            SelectUser();
        }
    }

    int SelectUser()
    {
        int i = 0;
        users = GetComponents<ResourceUser>();
        foreach (ResourceUser user in users)
        {
            if (i == selectedUser)
            {
                user.enabled = true;
                userIcon.sprite = user.icon;
                resourceIcon.sprite = user.resource.icon;
                if (inventory.resourceCount.ContainsKey(user.resource))
                    resourceCount.text = "× " + (inventory.resourceCount[user.resource]).ToString("0");
                else
                    resourceCount.text = "× 0";
            }
            else
                user.enabled = false;
            i++;
        }
        return i;
    }

    void ResourceCountChange(Item resource)
    {
        if (resource == users[selectedUser].resource)
        {
            if (inventory.resourceCount.ContainsKey(users[selectedUser].resource))
                resourceCount.text = "× " + (inventory.resourceCount[users[selectedUser].resource]).ToString("0");
        }
        else if (resource == frashlightUser.resource)
        {
            if (inventory.resourceCount.ContainsKey(frashlightUser.resource))
                batteryCount.text = "× " + (inventory.resourceCount[frashlightUser.resource]).ToString("0");
        }
    }
}