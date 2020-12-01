using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable : Interactable
{
    public GameObject miniGamePanel;

    public List<string> password;

    public Sprite originSprite;
    public Sprite doneSprite;

    public GameObject gifts;

    public override void Interact()
    {
        base.Interact();

        PlayMiniGame();
    }

    void PlayMiniGame()
    {
        GameManager.instance.DisablePlayer();
        miniGamePanel.SetActive(true);
        miniGamePanel.GetComponentInChildren<MiniGame>().SetPassword(password);
        miniGamePanel.GetComponentInChildren<MiniGame>().SetOwner(this);
    }

    public void SendGifts()
    {
        GameManager.instance.EnablePlayer();
        miniGamePanel.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = doneSprite;
        gifts.SetActive(true);
        this.enabled = false;
    }

    protected void OnEnable()
    {
        gifts.SetActive(false);
        shinning.SetActive(false);
    }
}
