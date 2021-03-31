using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float moveSpeed;
    public float minRadius = 0.5f;  // enemy will find player no matter what
    public float lookRadius;
    public float giveUpRadius;
	public float lookAngleDegree;
	public float rotateSpeed; // level
    public bool playBGMOrNot = true;

    protected bool checkSafeAreaOrNot = false;

    protected Vector2 movement;
	protected Rigidbody2D ownRb;

	public List<Transform> targets = new List<Transform>();
	protected Transform current_target;
	protected int current_target_index;
	protected Transform player;

	protected bool foundPlayerOrNot = false;

    protected bool isIncreaseRotation = true;
    protected float originalRotation;
    protected bool playerIsInSafeArea = false;

    protected Vector2 startPosition;

    protected EnemyState state;
    protected enum EnemyState
    {
        Chasing,
        LookingAround,
        Back,
        Default,
        Unactive
    }

    protected virtual void Start()
    {
        if (checkSafeAreaOrNot)
        {
            GameManager.instance.OnPlayerEnterSafeAreaCallBack += PlayerEnterSafeArea;
            GameManager.instance.OnPlayerLeaveSafeAreaCallBack += PlayerLeaveSafeArea;
        }
        startPosition = transform.position;
        GameManager.instance.onPlayerDieCallBack += ResetPosition;
    }

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
        if (distance <= lookRadius && !playerIsInSafeArea)
        {
	        Vector2 lookDir = (Vector2)player.position - ownRb.position;
	        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
	        float AdjAngle = AdjustDegree(angle - ownRb.rotation);
	        if(Mathf.Abs(AdjAngle) <= lookAngleDegree || distance <= minRadius)
	    	{
                if (!foundPlayerOrNot)
                {
                    if(playBGMOrNot)
                        GameManager.instance.FoundPlayer();
                    SoundManager.instance.Play("BeFound");
                    Debug.Log("Player be found");
                }
                foundPlayerOrNot = true;
	    	}
        }
        else if(distance >= giveUpRadius || playerIsInSafeArea)
        {
            if (foundPlayerOrNot && playBGMOrNot)
            {
                Debug.Log("Player be lost");
                GameManager.instance.LosePlayer();
            }
            foundPlayerOrNot = false;
        }
	}

    protected void LookingAround(float lookingAroundAngle)
    {
        if (isIncreaseRotation)
        {
            if(ownRb.rotation < originalRotation + lookingAroundAngle)
                ownRb.rotation += rotateSpeed/5;
            else
                isIncreaseRotation = false;
        }
        else
        {
            if (ownRb.rotation > originalRotation - lookingAroundAngle)
                ownRb.rotation -= rotateSpeed/5;
            else
                isIncreaseRotation = true;
        }
    }

	protected virtual void Disappear()
	{
        gameObject.SetActive(false);
	}

    protected void OnDrawGizmos()
    {
        for(int i = 0 ; i < targets.Capacity ; i++)
        {
            Gizmos.DrawLine(targets[i].position, targets[(i + 1) % targets.Capacity].position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minRadius);
    }

    private void OnEnable()
    {
        Heartbeat.instance.ScanEnemy();
    }

    protected void PlayerLeaveSafeArea()
    {
        playerIsInSafeArea = false;
    }

    protected void PlayerEnterSafeArea()
    {
        playerIsInSafeArea = true;
    }

    protected void ResetPosition()
    {
        transform.position = startPosition;
    }
}
