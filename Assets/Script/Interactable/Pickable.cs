using System.Collections;
using UnityEngine;

public class Pickable : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();

        PickUp();

        if (plotTrigger != null)
            StartCoroutine(WaitAndStartPlot());
    }

    void PickUp()
    {
        if (plotTrigger == null)
            gameObject.SetActive(false);
        SoundManager.instance?.Play("PickUp");
        Inventory.instance?.Add(item);
    }

    IEnumerator WaitAndStartPlot()
    {
        yield return new WaitForSeconds(0.1f);
        plotTrigger.TriggerPlot();
        gameObject.SetActive(false);
    }
}
