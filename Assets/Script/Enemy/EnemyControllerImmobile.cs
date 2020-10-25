using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerImmobile : MonoBehaviour
{

	public float lookRadius;
	public float rotateSpeed;

	Vector2 movement;
	Rigidbody2D ownRb;

	Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        ownRb = GetComponent<Rigidbody2D>();
    }

    IEnumerator Turn(float angle)
    {
    	Debug.Log("turn!");
    	float AdjAngle = AdjustDegree(angle - ownRb.rotation);
    	if(AdjAngle > 0){
    		if(AdjustDegree(angle - ownRb.rotation) > rotateSpeed){
    			ownRb.rotation = ownRb.rotation + rotateSpeed;
    			yield return 0;
    		}
    		else{
    			ownRb.rotation = angle;
    		}
    	}
    	else{
    		if(AdjustDegree(angle - ownRb.rotation) < -1 * rotateSpeed){
    			ownRb.rotation = ownRb.rotation - rotateSpeed;
    			yield return 0;
    		}
    		else{
    			ownRb.rotation = angle;
    		}
    	}
	}

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);

        if(distance <= lookRadius){
        	Vector2 target_position = target.position;
        	Vector2 lookDir = target_position - ownRb.position;
	        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
	        StartCoroutine("Turn", angle);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
