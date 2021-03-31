using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class ChaseEnemy : Enemy
{
    public float chaseSpeed;
    public float startChasingDelay;
    public float lookingAroundDuration;
    public float lookingAroundAngle;
    public float recoverDuration;

    float tempRotateSpeed;
	float timer1 = 0;
    float timer2 = 0;

    protected SAP2DAgent agent;

    protected override void Start()
    {
        state = EnemyState.Default;

        player = GameManager.instance.player.transform;
        current_target_index = 0;
        ownRb = GetComponent<Rigidbody2D>();
        agent = GetComponent<SAP2DAgent>();
        agent.MovementSpeed = moveSpeed;
        current_target = targets[current_target_index];
        agent.Target = current_target;
        tempRotateSpeed = agent.RotationSpeed;

        checkSafeAreaOrNot = true;

        base.Start();
    }

    void Update()
    {
        if (state == EnemyState.Unactive)
            return;

    	LookingForPlayer();

        if (foundPlayerOrNot)
    	{
    		if(state != EnemyState.Chasing)
    		{
    			if(timer1 < startChasingDelay)
    			{
                    if (timer1 == 0)
                    {
                        agent.enabled = false;
                    }
    				timer1 += Time.deltaTime;
    			}
    			else
    			{
                    state = EnemyState.Chasing;
                    agent.enabled = true;
                    agent.MovementSpeed = chaseSpeed;
                    agent.Target = player;
    				agent.RotationSpeed = 0;
                    timer1 = 0;
			    }
    		}

            if (agent.isActiveAndEnabled)
                agent.MovementSpeed = chaseSpeed;
    		StareAtPlayer();
    	}
    	else
    	{
            timer1 = 0;
            if (state == EnemyState.Chasing)
            {
                state = EnemyState.LookingAround;
                agent.enabled = false;
                originalRotation = ownRb.rotation;
            }
            else if (state == EnemyState.LookingAround)
            {
                if (timer2 < lookingAroundDuration)
                {
                    LookingAround(lookingAroundAngle);
                    timer2 += Time.deltaTime;
                }
                else
                {
                    state = EnemyState.Default;
                    agent.enabled = true;
                    agent.MovementSpeed = moveSpeed;
                    agent.RotationSpeed = tempRotateSpeed;
                    timer2 = 0;
                    agent.Target = current_target;
                }
            }
            else if (state == EnemyState.Default)
            {
                agent.enabled = true;
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

    public void SlowDown()
    {
        float originSpeed = chaseSpeed;
        chaseSpeed = 0;
        StartCoroutine(RecoverSpeed(recoverDuration, originSpeed));
    }

    IEnumerator RecoverSpeed(float recoverDuration, float originSpeed)
    {
        while (chaseSpeed < originSpeed)
        {
            chaseSpeed += (originSpeed / recoverDuration) * Time.deltaTime;
            yield return null;
        }
        chaseSpeed = originSpeed;
    }
}
