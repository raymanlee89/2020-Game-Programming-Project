using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerPatrol : MonoBehaviour
{
	public float moveSpeed;
	public float rotateSpeed;
	public bool moveToTarget1;
	public List<Transform> target = new List<Transform>();

	int CurrentTarget;
	int C;
	bool IsSwitching;
	Vector2 movement;
    Vector2 target_position;
    Rigidbody2D ownRb;

    // Start is called before the first frame update
    void Start()
    {
    	IsSwitching = false;
    	C = target.Capacity;
    	CurrentTarget = 0;
        ownRb = GetComponent<Rigidbody2D>();

        target_position = target[CurrentTarget].position;
    	movement = target_position - ownRb.position;
        movement = movement.normalized;
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
	    ownRb.rotation = angle;
	    
	    if(moveToTarget1){
	    	ownRb.MovePosition(target[0].position);
	    }
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsSwitching)
        {
            movement = target_position - ownRb.position;
            movement = movement.normalized;
            ownRb.MovePosition(ownRb.position + movement * moveSpeed * Time.deltaTime);

            float Dis = Vector2.Distance(target_position, ownRb.position);
		    if(Dis <= 0.01){
		    	Debug.Log("touched");
		    	ownRb.MovePosition(target_position);
		    	SwitchTarget();
		    }
		}
		else
        {
			//switching
		}
    }	

    IEnumerator Turn(float angle)
    {
    	Debug.Log("turn!");
    	IsSwitching = true;
    	float AdjAngle = AdjustDegree(angle - ownRb.rotation);
    	if(AdjAngle > 0)
        {
    		while(AdjustDegree(angle - ownRb.rotation) > rotateSpeed)
            {
    			ownRb.rotation = ownRb.rotation + rotateSpeed;
    			yield return 0;
    		}
    		ownRb.rotation = angle;
    	}
    	else
        {
    		while(AdjustDegree(angle - ownRb.rotation) < -1 * rotateSpeed)
            {
    			ownRb.rotation = ownRb.rotation - rotateSpeed;
    			yield return 0;
    		}
    		ownRb.rotation = angle;
    	}
    	IsSwitching = false;
		
    }

	void SwitchTarget()
    {
		CurrentTarget = (CurrentTarget + 1) % C;
		target_position = target[CurrentTarget].position;
    	movement = target_position - ownRb.position;
        movement = movement.normalized;
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
        StartCoroutine(Turn(angle));
	}

	void OnDrawGizmos(){
		if(target.Capacity >= 2)
		{
			for(int i=0 ; i<target.Capacity ; i++)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(target[i].position, target[(i+1) % target.Capacity].position);
			}
		}
	}

	float AdjustDegree(float angle)
    {
		float AdjAngle = angle;
		while(AdjAngle > 180){
			AdjAngle = AdjAngle - 360;
		}
		while(AdjAngle <= -180){
			AdjAngle = AdjAngle + 360;
		}
		return AdjAngle;
	}
}
