using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerImmobile : MonoBehaviour
{
	public float lookRadius;

	Vector2 movement;
	Rigidbody2D ownRb;

	Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        ownRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);

        if(distance <= lookRadius){
        	Vector2 target_position = target.position;
        	Vector2 lookDir = target_position - ownRb.position;
	        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
	        ownRb.rotation = angle;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
