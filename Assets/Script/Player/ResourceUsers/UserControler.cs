﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserControler : MonoBehaviour
{
    public Sprite defaultImage;
    public Sprite defaultWithFrashlightImage;
    public Sprite changingImage;
    public Sprite changingWithFrashlightImage;
    public FrashlightUser frashlightUser;
    bool frashlightUserHasBeenEnabled = false;
    ResourceUser[] allUsers = null;
    [HideInInspector]
    public List<ResourceUser> availableUsers = new List<ResourceUser>();
    Inventory inventory;
    [HideInInspector]
    public int selectedUser = 0;

    #region Singleton
    public static UserControler instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of UserControler found!");
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
        inventory.onGearChangedCallBack += OpenResourceUser;
        frashlightUser = GetComponent<FrashlightUser>();
        allUsers = GetComponents<ResourceUser>();
        frashlightUser.enabled = false;
        UIManager.instance?.CloseResourceUserUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (availableUsers.Count <= 1)
            return;

        int previousSelectedUser = selectedUser;
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedUser = (selectedUser + 1) % availableUsers.Count;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedUser--;
            if (selectedUser < 0)
                selectedUser = availableUsers.Count - 1;
        }

        if (Input.GetButtonDown("Gear1") && availableUsers.Count >= 1)
            selectedUser = 0;
        if (Input.GetButtonDown("Gear2") && availableUsers.Count >= 2)
            selectedUser = 1;
        if (Input.GetButtonDown("Gear3") && availableUsers.Count >= 3)
            selectedUser = 2;
        if (Input.GetButtonDown("Gear4") && availableUsers.Count >= 4)
            selectedUser = 3;

        if (previousSelectedUser != selectedUser || availableUsers.Count == 1)
        {
            SoundManager.instance?.Play("ChangeUser");
            SelectUser();
        }
    }

    void SelectUser()
    {
        int i = 0;
        foreach (ResourceUser user in availableUsers)
        {
            if (i == selectedUser)
            {
                user.enabled = true;
            }
            else
                user.enabled = false;
            i++;
        }

        UpdateResourceUserUI();
    }

    void ResourceCountChange(Item resource)
    {
        if(resource == null) // reset data
        {
            UIManager.instance?.UpdateBatteryCount(frashlightUser.resource);
            UpdateResourceUserUI();
        }

        if (resource == frashlightUser.frashlightItem)
        {
            frashlightUser.enabled = true;
            frashlightUserHasBeenEnabled = true;
        }
        else if (resource == frashlightUser.resource)
        {
            UIManager.instance?.UpdateBatteryCount(frashlightUser.resource);
        }

        if (availableUsers.Count == 0)
            return;
        
        if (resource == availableUsers[selectedUser].resource)
        {
            UpdateResourceUserUI();
        }
    }

    void UpdateResourceUserUI()
    {
        if (availableUsers.Count == 1)
            UIManager.instance?.UpdateResourceUserUI(availableUsers[selectedUser], null, null);
        else if (availableUsers.Count != 0)
        {
            int former = selectedUser - 1;
            if (former < 0)
                former = availableUsers.Count - 1;
            int later = (selectedUser + 1) % availableUsers.Count;
            if (former == later)
                UIManager.instance?.UpdateResourceUserUI(availableUsers[selectedUser], null, availableUsers[later]);
            else
                UIManager.instance?.UpdateResourceUserUI(availableUsers[selectedUser], availableUsers[former], availableUsers[later]);
        }
    }

    void OpenResourceUser(Item resource)
    {
        Debug.Log("Open user" + resource.itemData.name);
        foreach(ResourceUser user in allUsers)
        {
            if (resource == user.resource)
                availableUsers.Add(user);
        }

        if (availableUsers.Count == 1)
            UIManager.instance?.OpenResourceUserUI();

        SelectUser();
    }

    void OnDisable()
    {
        if(availableUsers.Count != 0)
            availableUsers[selectedUser].enabled = false;
    }

    void OnEnable()
    {
        if (availableUsers.Count != 0)
            availableUsers[selectedUser].enabled = true;
    }
}