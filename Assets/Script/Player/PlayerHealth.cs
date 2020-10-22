using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    float health;
    CameraController cameraController;

    void Start()
    {
        health = maxHealth;
        UIManager.instance?.healthBar.SetMaxFill(maxHealth);
        cameraController = GetComponent<CameraController>();
    }

    public void GetHurt(float damage)
    {
        Debug.Log("Get hurt!");
        SoundManager.instance?.Play("GetHurt");
        cameraController.CameraShake();
        health -= damage;
        if(health <= 0)
        {
            health = 0;
            // died and respawn
            // loading data
        }
        UIManager.instance?.healthBar.SetFill(health);
    }

    public void Healing(float healingAmount)
    {
        SoundManager.instance?.Play("Healing");
        health += healingAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UIManager.instance?.healthBar.SetFill(health);
    }

    public bool IsFullHealth()
    {
        if (health == maxHealth)
            return true;
        return false;
    }
}
