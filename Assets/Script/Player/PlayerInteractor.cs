using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable targetItem = collision.GetComponent<Interactable>();
        if(targetItem != null && targetItem.isActiveAndEnabled)
        {
            targetItem.touchable = true;
            targetItem.OpenShinning();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Interactable targetItem = collision.GetComponent<Interactable>();
        if (targetItem != null && targetItem.isActiveAndEnabled)
        {
            targetItem.touchable = false;
            targetItem.CloseShinning();
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interact!!");
    }
}
