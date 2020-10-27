using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaringArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if(playerMovement != null)
            {
                playerMovement.StartBeingStared();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.StopBeingStared();
            }
        }
    }
}
