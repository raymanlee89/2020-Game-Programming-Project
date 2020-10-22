using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerPatrol : MonoBehaviour
{
	public float moveSpeed;
	public List<Transform> target = new List<Transform>();
	public float cd;

	int CurrentTarget;
	int C;
	bool OnCoolDown;
	float CoolDownLeft;
	Vector2 movement;
	Vector2 Movement;
	Rigidbody2D ownRb;

    // Start is called before the first frame update
    void Start()
    {
    	OnCoolDown = false;
    	C = target.Capacity;
    	CurrentTarget = 0;
        ownRb = GetComponent<Rigidbody2D>();
        Vector2 target_position = target[CurrentTarget].position;
    	Movement = target_position - ownRb.position;
        movement = Movement.normalized;
    }

    // Update is called once per frame
    void Update()
    {
    	if(OnCoolDown)
        {
            if(CoolDownLeft > 0)
                CoolDownLeft -= Time.deltaTime;
            else
            {
                CoolDownLeft = 0;
                OnCoolDown = false;
            }
        }

        Vector2 target_position = target[CurrentTarget].position;
        float Dis = Vector2.Distance(target_position, ownRb.position);
        ownRb.MovePosition(ownRb.position + movement * moveSpeed * Time.fixedDeltaTime);
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
        ownRb.rotation = angle;

        //Debug.Log(Dis);

        if (Dis <= 0.01 && !OnCoolDown)
        {
            ownRb.MovePosition(target_position);
            SwitchTarget();
            OnCoolDown = true;
            CoolDownLeft = cd;
        }
    }

	void SwitchTarget()
    {
        //Debug.Log(CurrentTarget);
		CurrentTarget = (CurrentTarget + 1) % C;
		Vector2 target_position = target[CurrentTarget].position;
    	Movement = target_position - ownRb.position;
        movement = Movement.normalized;
	}

    private void OnDrawGizmos()
    {
        if (target.Capacity != 0)
        {
            for (int i = 0; i < target.Capacity; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(target[i].position, target[(i + 1) % target.Capacity].position);
                Gizmos.DrawIcon(target[i].position, "item.png", true);
            }
        }
    }
}
