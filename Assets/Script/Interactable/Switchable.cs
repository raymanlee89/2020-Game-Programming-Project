using UnityEngine;

public class Switchable : Interactable
{
    public GameObject switchableThing;
    public DialogueTrigger howToTurnOnPowerDialogue;

    public override void Interact()
    {
        base.Interact();

        if(PlotManager.instance.powerIsOnOrNot)
            Switch();
        else
        {
            SoundManager.instance.Play("ClickLightSwitch");
            howToTurnOnPowerDialogue.TriggerDialogue();
        }
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
