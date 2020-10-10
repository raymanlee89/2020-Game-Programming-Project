using UnityEngine;

public class Switchable : Interactable
{
    public GameObject switchableThing;

    public override void Interact()
    {
        base.Interact();

        Switch();
    }

    void Switch()
    {
        if (!switchableThing.activeSelf)
            switchableThing.SetActive(true);
        else
            switchableThing.SetActive(false);
        SoundManager.instance.Play("ClickLightSwitch");
    }
}
