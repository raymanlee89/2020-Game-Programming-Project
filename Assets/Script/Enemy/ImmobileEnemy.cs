using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmobileEnemy : Enemy
{
    public float lookingAroundAngle;

    void Start()
    {
        player = GameManager.instance.player.transform;
        ownRb = GetComponent<Rigidbody2D>();
        originalRotation = ownRb.rotation;
    }

    void Update()
    {
        LookingForPlayer();
        if(lookingAroundAngle > 0)
            LookingAround(lookingAroundAngle);

        if(foundPlayerOrNot)
        {
            StareAtPlayer();
        }
    }
}
