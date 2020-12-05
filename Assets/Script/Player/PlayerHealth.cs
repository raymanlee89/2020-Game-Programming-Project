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
            Die();
        }
        UIManager.instance?.healthBar.SetFill(health);
    }

    void Die()
    {
        UIManager.instance.OpenLossPanel();
    }

    public void Healing(float healingAmount)
    {
        health += healingAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UIManager.instance?.healthBar.SetFill(health);
    }

    public void RecoverHealth()
    {
        health = maxHealth;
        UIManager.instance?.healthBar.SetFill(health);
    }

    public bool IsFullHealth()
    {
        if (health >= maxHealth)
            return true;
        return false;
    }
}
