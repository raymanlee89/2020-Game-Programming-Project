using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class PatrolEnemy : Enemy
{
	SAP2DAgent agent;

	void Start()
	{
    	current_target_index = 0;
        ownRb = GetComponent<Rigidbody2D>();
        agent = GetComponent<SAP2DAgent>();
        agent.MovementSpeed = moveSpeed;
        agent.Target = targets[current_target_index];
        player = GameManager.instance.player.transform;
	}

	void Update()
    {
    	LookingForPlayer();
    	if(foundPlayerOrNot)
    	{
			agent.enabled = false;		
    		StareAtPlayer();
    	}
    	else
    	{
    		agent.enabled = true;
	    	float Dis = Vector2.Distance(agent.Target.position, ownRb.position);
		    if(Dis <= 0.2)
		    {
		    	//ownRb.MovePosition(agent.Target.position);
		    	current_target_index = (current_target_index + 1) % targets.Capacity;
		    	agent.Target = targets[current_target_index];
		    }
		}
    }
}
