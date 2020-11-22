using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.playerIsInSafeAreaOrNot = true;
            Debug.Log("Player is safe right now.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.playerIsInSafeAreaOrNot = false;
            Debug.Log("Player is not safe right now.");
        }
    }
}
