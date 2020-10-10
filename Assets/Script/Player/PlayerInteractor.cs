using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable targetItem = collision.GetComponent<Interactable>();
        if(targetItem != null)
        {
            targetItem.touchable = true;
            targetItem.shinning.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Interactable targetItem = collision.GetComponent<Interactable>();
        if (targetItem != null)
        {
            targetItem.touchable = false;
            targetItem.shinning.SetActive(false);
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interact!!");
    }
}
