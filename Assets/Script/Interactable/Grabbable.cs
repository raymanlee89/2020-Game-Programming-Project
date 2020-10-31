using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    bool isGrabbed = false;
    PlayerInteractor playerInteractor;
    Transform originalParent;

    private void Start()
    {
        originalParent = transform.parent;
    }

    public override void Interact()
    {
        base.Interact();

        if (isGrabbed)
        {
            isGrabbed = false;
            transform.parent = originalParent;
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else
        {
            isGrabbed = true;
            transform.parent = playerInteractor.transform;
            transform.position = playerInteractor.transform.position;
            transform.rotation = playerInteractor.transform.rotation;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interacter")
            playerInteractor = collision.GetComponent<PlayerInteractor>();
    }
}
