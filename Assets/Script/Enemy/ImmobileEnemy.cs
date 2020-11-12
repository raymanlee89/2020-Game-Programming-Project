using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmobileEnemy : Enemy
{
    void Start()
    {
        rotateSpeed = rotateSpeed / 5;
        player = PlayerManager.instance.player.transform;
        ownRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        LookingForPlayer();

        if(foundPlayerOrNot)
        {
            StareAtPlayer();
        }
    }
}
