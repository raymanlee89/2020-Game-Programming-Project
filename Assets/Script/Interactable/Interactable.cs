using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject shinning;

    [HideInInspector]
    public bool touchable = false;

    void Start()
    {
        shinning.SetActive(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Interact") && touchable)
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        //Debug.Log("Item is interacted");
    }
}
