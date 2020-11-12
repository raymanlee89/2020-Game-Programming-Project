using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class Enemy : MonoBehaviour
{
	public float moveSpeed;
	public float lookRadius;
	public float lookAngleDegree;
	public float rotateSpeed; // level

	protected Vector2 movement;
	protected Rigidbody2D ownRb;

	public List<Transform> targets = new List<Transform>();
	protected Transform current_target;
	protected int current_target_index;
	protected Transform player;

	protected bool foundPlayerOrNot = false;

	protected Vector2[] path;
    protected SAP2DPathfinder pathfinder;

    protected void StareAtPlayer()
    {
    	Vector2 lookDir = (Vector2)player.position - ownRb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    	float AdjAngle = AdjustDegree(angle - ownRb.rotation);
    	if(Mathf.Abs(AdjAngle) > rotateSpeed)
    	{
    		ownRb.rotation += Mathf.Sign(AdjAngle) * rotateSpeed;
    	}
    	else
    	{
    		ownRb.rotation = angle;
    	}
    }

    protected float AdjustDegree(float angle)
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

	protected void LookingForPlayer()
	{
		float distance = Vector2.Distance(player.position, transform.position);

        if(distance <= lookRadius)
        {
	        Vector2 lookDir = (Vector2)player.position - ownRb.position;
	        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
	        float AdjAngle = AdjustDegree(angle - ownRb.rotation);
	        if(Mathf.Abs(AdjAngle) <= lookAngleDegree)
	    	{
	    		foundPlayerOrNot = true;
	    	}
        }
        else if(distance >= 2 * lookRadius)
        {
        	foundPlayerOrNot = false;
        }
	}

	protected void Disappear()
	{

		Destroy(gameObject);
	}

    protected void OnDrawGizmos()
    {
        for(int i = 0 ; i < targets.Capacity ; i++)
        {
            Gizmos.DrawLine(targets[i].position, targets[(i + 1) % targets.Capacity].position);
        }

        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
