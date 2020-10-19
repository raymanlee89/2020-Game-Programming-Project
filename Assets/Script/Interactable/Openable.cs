using UnityEngine;

public class Openable : Interactable
{
    public bool isLocked = true;
    public Item key;
    public GameObject openedObject;
    public GameObject closedObject;

    void Start()
    {
        openedObject.SetActive(false);
        closedObject.SetActive(true);
        if (key == null)
            isLocked = false;
    }

    public override void Interact()
    {
        base.Interact();

        if (closedObject.activeSelf && !isLocked)
            Open();
        else if (closedObject.activeSelf && isLocked)
            TryToUnlock();
        else if (openedObject.activeSelf)
            Close();
    }

    void Open()
    {
        Debug.Log("Open");
        SoundManager.instance?.Play("OpenDoor");
        openedObject.SetActive(true);
        closedObject.SetActive(false);
    }

    void Close()
    {
        Debug.Log("Close");
        SoundManager.instance?.Play("CloseDoor");
        openedObject.SetActive(false);
        closedObject.SetActive(true);
    }

    void TryToUnlock()
    {
        Debug.Log("Try to unlock");
        if(Inventory.instance.clues.Contains(key))
        {
            SoundManager.instance?.Play("Unlock");
            isLocked = false;
            Open();
            return;
        }
        SoundManager.instance?.Play("TryToUnlock");
        return;
    }
}
