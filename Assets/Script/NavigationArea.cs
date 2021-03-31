using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArea : MonoBehaviour
{
    public string positionName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIManager.instance?.UpdatePositionText(positionName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIManager.instance?.UpdatePositionText(null);
        }
    }
}
