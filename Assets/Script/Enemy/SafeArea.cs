using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance?.PlayerEnterSafeArea();
            Debug.Log("Player is safe right now.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance?.PlayerLeaveSafeArea();
            Debug.Log("Player is not safe right now.");
        }
    }
}
