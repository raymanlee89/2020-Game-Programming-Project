using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerPatrol : MonoBehaviour
{
	public float moveSpeed;
	public float rotateSpeed;
	//public bool moveToTarget1;
	public List<Transform> target = new List<Transform>();

    protected int currentTargetIndex;
	protected bool IsTurning = false;
    protected Vector2 movement;
    protected Vector2 target_position;
    protected Rigidbody2D ownRb;

    // Start is called before the first frame update
    void Start()
    {
    	currentTargetIndex = 0;
        ownRb = GetComponent<Rigidbody2D>();

        target_position = target[currentTargetIndex].position;
    	movement = target_position - ownRb.position;
        movement = movement.normalized;
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
        ownRb.rotation = angle;

	    ownRb.MovePosition(target[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTurning)
        {
            movement = target_position - ownRb.position;
            movement = movement.normalized;
            ownRb.MovePosition(ownRb.position + movement * moveSpeed * Time.deltaTime);

            float Dis = Vector2.Distance(target_position, ownRb.position);
            if (Dis <= 0.01f)
            {
                Debug.Log("touched");
                ownRb.MovePosition(target_position);
                StartCoroutine(Turn());
            }
        }
    }

    protected void ChangeTarget ()
    {
        currentTargetIndex = (currentTargetIndex + 1) % target.Capacity;
        target_position = target[currentTargetIndex].position;
        movement = target_position - ownRb.position;
        movement = movement.normalized;
    }

    protected IEnumerator Turn()
    {
        IsTurning = true;

        ChangeTarget();

        // turnning
        Debug.Log("turn!");
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
        float AdjAngle = AdjustDegree(angle - ownRb.rotation);
        while(Mathf.Abs(AdjAngle) > rotateSpeed)
        {
            ownRb.rotation = ownRb.rotation + Mathf.Sign(AdjAngle) * rotateSpeed;
            AdjAngle = AdjustDegree(angle - ownRb.rotation);
            yield return null;
        }
        ownRb.rotation = angle;

    	IsTurning = false;
    }

    protected void OnDrawGizmos(){
		if(target.Capacity >= 2)
		{
			for(int i=0 ; i<target.Capacity ; i++)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(target[i].position, target[(i+1) % target.Capacity].position);
			}
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
}
