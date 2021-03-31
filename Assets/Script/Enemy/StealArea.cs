using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealArea : MonoBehaviour
{
    public float stealTime;

	bool inArea = false;
	bool isStealing = false;
	float timer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            UIManager.instance?.stealBar.SetMaxFill(stealTime);
            UIManager.instance?.stealBar.SetFill(timer);
            inArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = false;
        }
    }

    void Update()
    {
    	if(inArea && Input.GetButtonDown("Interact"))
    	{
    		isStealing = true;
            UIManager.instance?.stealBarPanel.SetActive(true);
            Debug.Log("stealing");
    	}
    	if((!inArea || Input.GetButtonUp("Interact")) && isStealing)
    	{
    		isStealing = false;
            UIManager.instance?.stealBarPanel.SetActive(false);
            Debug.Log("stop stealing");
        }

    	if(isStealing)
    	{
    		if(timer <= stealTime && timer >= 0)
    		{
    			timer += Time.deltaTime;
    			Debug.Log(timer);
                UIManager.instance?.stealBar.SetFill(timer);
            }
    		if(timer > stealTime && GameManager.instance.isPlayerAlive)
    		{
                timer = -1;
                UIManager.instance?.stealBarPanel.SetActive(false);
                GetComponentInParent<Instructor>()?.ChangeStage();
                this.enabled = false;
            }
    	}
    	else
    	{
            timer = 0;
        }
    }
}
