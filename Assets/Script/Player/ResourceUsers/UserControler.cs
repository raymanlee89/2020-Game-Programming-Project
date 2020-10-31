using System.Collections;
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
    ResourceUser[] allUsers = null;
    List<ResourceUser> availableUsers = new List<ResourceUser>();
    Inventory inventory;
    int selectedUser = 0;

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
        if (availableUsers.Count == 0)
            return;

        int previousSelectedUser = selectedUser;
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedUser = (selectedUser + 1) % availableUsers.Count;
            SoundManager.instance?.Play("ChangeUser");
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedUser--;
            SoundManager.instance?.Play("ChangeUser");
            if (selectedUser < 0)
                selectedUser = availableUsers.Count - 1;
        }

        if (previousSelectedUser != selectedUser || availableUsers.Count == 1)
        {
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
        if (resource == frashlightUser.frashlightItem)
        {
            frashlightUser.enabled = true;
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
        foreach(ResourceUser user in allUsers)
        {
            if (resource == user.resource)
                availableUsers.Add(user);
        }

        if (availableUsers.Count == 1)
            UIManager.instance?.OpenResourceUserUI();

        UpdateResourceUserUI();
    }
}