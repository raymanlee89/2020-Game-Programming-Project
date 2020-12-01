using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject shinning;
    public PlotTrigger plotTrigger = null;

    [HideInInspector]
    public bool touchable = false;

    private void OnDisable()
    {
        CloseShinning();
    }

    private void OnEnable()
    {
        CloseShinning();
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

    public void OpenShinning()
    {
        shinning.SetActive(true);
    }

    public void CloseShinning()
    {
        shinning.SetActive(false);
    }
}
