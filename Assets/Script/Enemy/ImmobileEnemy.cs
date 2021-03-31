using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmobileEnemy : Enemy
{
    public float lookingAroundAngle;

    protected override void Start()
    {
        player = GameManager.instance.player.transform;
        ownRb = GetComponent<Rigidbody2D>();
        originalRotation = ownRb.rotation;

        state = EnemyState.Default;

        base.Start();
    }

    void Update()
    {
        LookingForPlayer();
        if(lookingAroundAngle > 0 && !foundPlayerOrNot)
            LookingAround(lookingAroundAngle);

        if(foundPlayerOrNot)
        {
            StareAtPlayer();
        }
    }
}
