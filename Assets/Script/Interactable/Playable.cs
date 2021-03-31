using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable : Interactable
{
    public GameObject miniGamePanel;
    public DialogueTrigger howToUnlockHint;
    public bool isLockByDefault = false;

    public List<string> password;

    public Sprite originSprite;
    public Sprite doneSprite;

    public GameObject gifts = null;
    public bool playOpenSound = true;
    bool ableToPlay = true;

    public override void Interact()
    {
        base.Interact();

        if(!DialogueManager.instance.IsDialogueStarted() && ableToPlay)
        {
            ableToPlay = false;
            if (isLockByDefault)
                PlayMiniGame();
            else
                SendGifts();
        }
    }

    void PlayMiniGame()
    {
        GameManager.instance?.DisablePlayer();
        miniGamePanel.SetActive(true);
        miniGamePanel.GetComponentInChildren<MiniGame>().SetOwner(this);
        miniGamePanel.GetComponentInChildren<MiniGame>().SetPassword(password);
    }

    public void SendGifts()
    {
        GameManager.instance?.EnablePlayer();
        if(playOpenSound)
            SoundManager.instance.Play("OpenDoor");
        miniGamePanel.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = doneSprite;
        if(gifts != null)
            gifts.SetActive(true);
        GameManager.instance?.SaveGame();
        this.enabled = false;
    }

    protected void OnEnable()
    {
        gifts.SetActive(false);
        CloseShinning();
    }

    public void PausePlay()
    {
        if (!gifts.activeSelf)
            howToUnlockHint?.TriggerDialogue();
        GameManager.instance?.EnablePlayer();
        miniGamePanel?.SetActive(false);
        ableToPlay = true;
    }

    public void ResetGifts()
    {
        GameObject [] giftEntities = gifts.GetComponentsInChildren<GameObject>();
        foreach(GameObject gift in giftEntities)
        {
            gift.SetActive(true);
        }
    }
}
