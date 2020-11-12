using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class ChaseEnemy : Enemy
{
	bool IsChasing;
    public float chaseSpeed;
    float tempRotateSpeed;
	float timer;

	SAP2DAgent agent;

	void Start()
	{
		timer = 0;
    	IsChasing = false;
    	player = PlayerManager.instance.player.transform;
    	current_target_index = 0;
        ownRb = GetComponent<Rigidbody2D>();
        agent = GetComponent<SAP2DAgent>();
        agent.MovementSpeed = moveSpeed;
        current_target = targets[current_target_index];
        agent.Target = current_target;
        tempRotateSpeed = agent.RotationSpeed;
    }

	void Update()
    {	
    	LookingForPlayer();
    	if(foundPlayerOrNot)
    	{
    		if(!IsChasing)
    		{
    			if(timer < 1)
    			{
    				agent.enabled = false;
    				timer += Time.deltaTime;
    			}
    			else
    			{
                    IsChasing = true;
    				agent.enabled = true;
                    agent.MovementSpeed = chaseSpeed;
                    agent.Target = player;
    				agent.RotationSpeed = 0;
			    }
    		}

    		StareAtPlayer();
    	}
    	else
    	{
            if (IsChasing)
            {
                IsChasing = false;
    		    agent.enabled = true;
                agent.MovementSpeed = moveSpeed;
                agent.RotationSpeed = tempRotateSpeed;
    		    timer = 0;
    		    agent.Target = current_target;
            }
	    	float Dis = Vector2.Distance(agent.Target.position, ownRb.position);
		    if(Dis <= 0.2)
		    {
		    	//ownRb.MovePosition(agent.Target.position);
		    	current_target_index = (current_target_index + 1) % targets.Capacity;
		    	current_target = targets[current_target_index];
		    	agent.Target = current_target;
		    }
    	}
        
    }
}
