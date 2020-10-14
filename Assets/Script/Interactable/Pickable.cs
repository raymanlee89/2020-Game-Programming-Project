using UnityEngine;

public class Pickable : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        Destroy(gameObject);
        SoundManager.instance?.Play("PickUp");
        Inventory.instance?.Add(item);
    }
}
