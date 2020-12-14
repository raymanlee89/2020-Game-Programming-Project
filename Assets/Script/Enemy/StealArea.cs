using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealArea : MonoBehaviour
{
	public GameObject currentInstructor;
	public GameObject nextInstructor;
	public string Stage;

	bool inArea = false;
	bool isStealing = false;
	float timer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
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
    	if(inArea && Input.GetButtonDown("Steal"))
    	{
    		isStealing = true;
    		Debug.Log("stealing");
    	}
    	if(!inArea || Input.GetButtonUp("Steal"))
    	{
    		isStealing = false;
    	}

    	if(isStealing)
    	{
    		if(timer <= 3)
    		{
    			timer += Time.deltaTime;
    			Debug.Log(timer);
    		}
    		if(timer > 3)
    		{
    			string s = "Stage " + Stage + " Clear!";
    			Debug.Log(s);
    			currentInstructor.SetActive(false);
    			if(Stage != "3")
    			{
    				nextInstructor.SetActive(true);
    			}
    		}
    	}
    	else
    	{
    		timer = 0;
    	}
    }
}
