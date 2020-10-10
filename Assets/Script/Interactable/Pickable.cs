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
        if (SoundManager.instance != null)
            SoundManager.instance.Play("PickUp");
        if(Inventory.instance != null)
            Inventory.instance.Add(item);
    }
}
