using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkable : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();

        Check();
    }

    void Check()
    {
        SoundManager.instance?.Play("PickUp");
        UIManager.instance?.OpenCluePanel(item);
    }
}
