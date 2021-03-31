using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicOpenable : Interactable
{
    public bool isLocked = true;
    public bool isClosedByDefalut = true;
    [HideInInspector]
    public bool isOpen = false;
    public GameObject openedObject;
    public GameObject closedObject;
    public GameObject miniGamePanel;
    public DialogueTrigger howToUnlockDialogue;
    public DialogueTrigger howToTurnOnPowerDialogue;
    bool waitToOpen = false;

    public List<string> password;

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
        if (PlotManager.instance.powerIsOnOrNot)
        {
            PlayMiniGame();
        }
        else
        {
            SoundManager.instance?.Play("TryToUnlockDoor");
            howToTurnOnPowerDialogue.TriggerDialogue();
        }
    }

    public void Unlock()
    {
        SoundManager.instance?.Play("UnlockDoor");
        isLocked = false;
        StartCoroutine(WaitAndOpen());
        GameManager.instance.EnablePlayer();
    }

    IEnumerator WaitAndOpen()
    {
        Debug.Log("WaitAndOpen");
        waitToOpen = true;
        yield return new WaitForSeconds(1f);
        Open();
        waitToOpen = false;
    }

    public void Reset(bool open)
    {
        if (open)
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

    void PlayMiniGame()
    {
        GameManager.instance.DisablePlayer();
        miniGamePanel.SetActive(true);
        miniGamePanel.GetComponentInChildren<NumberLock>().SetPassword(password);
        miniGamePanel.GetComponentInChildren<NumberLock>().SetOwner(this);
    }

    public void PausePlay()
    {
        howToUnlockDialogue.TriggerDialogue();
        GameManager.instance.EnablePlayer();
        miniGamePanel.SetActive(false);
    }
}
