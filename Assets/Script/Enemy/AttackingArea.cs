using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingArea : MonoBehaviour
{
    public float damage;
    public float attackingCD;
    bool attackCoolingOrNot = false;
    float attackingCDLeft;

    void Start()
    {
        attackingCDLeft = 0;
    }

    void Update()
    {
        if(attackCoolingOrNot)
        {
            if(attackingCDLeft > 0)
                attackingCDLeft -= Time.deltaTime;
            else
            {
                attackingCDLeft = 0;
                attackCoolingOrNot = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !attackCoolingOrNot)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Attack!");
                playerHealth.GetHurt(damage);
                attackCoolingOrNot = true;
                attackingCDLeft = attackingCD;
                SoundManager.instance?.Play("Attack");
            }
        }
    }
}
