using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class StandAndChaseEnemy : Enemy
{
    public float chaseSpeed;
    public float startChasingDelay;
    public float recoverDuration;
    public float lookingAroundAngle;
    public float lookingAroundDuration;

    float tempRotateSpeed;
    float timer1 = 0;
    float timer2 = 0;

    SAP2DAgent agent;

    float tempOriginalRotation;

    protected override void Start()
    {
        player = GameManager.instance.player.transform;
        ownRb = GetComponent<Rigidbody2D>();
        originalRotation = ownRb.rotation;
        tempOriginalRotation = originalRotation;

        current_target = targets[0];

        state = EnemyState.Default;

        agent = GetComponent<SAP2DAgent>();
        agent.MovementSpeed = 0;
        agent.Target = null;
        agent.enabled = false;
        tempRotateSpeed = agent.RotationSpeed;

        checkSafeAreaOrNot = true;

        base.Start();
    }

    void Update()
    {
        LookingForPlayer();

        if (foundPlayerOrNot)
        {
            if (state != EnemyState.Chasing)
            {
                if (timer1 < startChasingDelay)
                {
                    timer1 += Time.deltaTime;
                }
                else
                {
                    state = EnemyState.Chasing;
                    agent.enabled = true;
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
                    state = EnemyState.Back;
                    agent.enabled = true;
                    agent.MovementSpeed = moveSpeed;
                    agent.RotationSpeed = tempRotateSpeed;
                    timer2 = 0;
                    agent.Target = current_target;
                }
            }
            else if (state == EnemyState.Back)
            {
                float Dis = Vector2.Distance(agent.Target.position, ownRb.position);
                if (Dis <= 0.2 && agent.isActiveAndEnabled)
                {
                    state = EnemyState.Default;
                    agent.MovementSpeed = 0;
                    agent.enabled = false;
                    originalRotation = tempOriginalRotation;
                }
            }
            else if (state == EnemyState.Default)
            {
                if (lookingAroundAngle > 0)
                    LookingAround(lookingAroundAngle);
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
