using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerChase : EnemyControllerPatrol
{
	public float chaseDis;
	public float chaseRange;
	public float chaseSpeed;

	bool IsChasing = false;
    Transform player;

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

        player = PlayerManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {	
    	if(!IsChasing){
	        if(!IsTurning)
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

                float P_Dis = Vector2.Distance(player.position, ownRb.position);

			    if(P_Dis <= chaseDis){
			    	Debug.Log("chase!");
			    	float DirBeforeChase = ownRb.rotation;
			    	Vector2 positionBeforeChase = ownRb.position;
			    	chase(DirBeforeChase, positionBeforeChase);
			    }
			}
			else{
				//switching
			}
		}
		else{

		}
    }	

	IEnumerator Chase(object[] param)
    {
		float DirBeforeChase = (float)param[0];
		Vector2 positionBeforeChase = (Vector2)param[1];
		IsChasing = true;
		yield return new WaitForSeconds(1);
		float T_Dis = Vector2.Distance(target[currentTargetIndex].position, ownRb.position);
		float P_Dis = Vector2.Distance(player.position, ownRb.position);
		while(T_Dis <= chaseRange && P_Dis < 2*chaseDis){
			Vector2 player_position = player.position;
			Vector2 Dir = player_position - ownRb.position;
			Dir = Dir.normalized;
			float angle2 = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg - 90f;
			ownRb.rotation = angle2;
			ownRb.MovePosition(ownRb.position + Dir * chaseSpeed / 30);
			T_Dis = Vector2.Distance(target[currentTargetIndex].position, ownRb.position);
			P_Dis = Vector2.Distance(player.position, ownRb.position);
			yield return null;
		}
		Vector2 target_position = target[currentTargetIndex].position;
        movement = target_position - ownRb.position;
        movement = movement.normalized;
		if(P_Dis > chaseDis){
			float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
			ownRb.rotation = angle;
		}
		IsChasing = false;
	}

	void chase(float DirBeforeChase, Vector2 positionBeforeChase){
		object[] param = new object[2];
		param[0] = DirBeforeChase;
		param[1] = positionBeforeChase;
		StartCoroutine("Chase", param);
	}

}
