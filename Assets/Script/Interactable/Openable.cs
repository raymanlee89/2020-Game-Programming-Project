using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : Interactable
{
    public bool isLocked = true;
    public bool isClosedByDefalut = true;
    [HideInInspector]
    public bool isOpen = false;
    public Item key;
    public GameObject openedObject;
    public GameObject closedObject;
    public DialogueTrigger howToUnlockDialogue;
    bool waitToOpen = false;

    void Start()
    {
        shinning.SetActive(false);

        if (isClosedByDefalut)
        {
            openedObject.SetActive(false);
            closedObject.SetActive(true);
        }
        else
        {
            openedObject.SetActive(true);
            closedObject.SetActive(false);
            isOpen = true;
        }
        
        if (key == null)
            isLocked = false;
    }

    public override void Interact()
    {
        base.Interact();

        if (closedObject.activeSelf && !isLocked && !waitToOpen)
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
        isOpen = true;
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
        if(Inventory.instance.ContainClue(key) || Inventory.instance.ContainResource(key))
        {
            SoundManager.instance?.Play("Unlock");
            isLocked = false;
            StartCoroutine(WaitAndOpen());
            return;
        }
        SoundManager.instance?.Play("TryToUnlock");
        howToUnlockDialogue.TriggerDialogue();
        return;
    }

    IEnumerator WaitAndOpen()
    {
        waitToOpen = true;
        yield return new WaitForSeconds(1f);
        Open();
        waitToOpen = false;
    }

    public void Reset(bool open)
    {
        if(open)
        {
            openedObject.SetActive(true);
            closedObject.SetActive(false);
            isOpen = true;
        }
        else
        {
            openedObject.SetActive(false);
            closedObject.SetActive(true);
            isOpen = false;
        }
    }
}
