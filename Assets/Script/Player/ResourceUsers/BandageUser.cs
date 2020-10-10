using UnityEngine;

public class BandageUser : ResourceUser
{
    PlayerHealth playerHealth;
    public float healingPercentage;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    protected override bool ItemAct()
    {
        if (playerHealth.IsFullHealth())
            return false;
        playerHealth.Healing(playerHealth.maxHealth * healingPercentage);
        return true;
    }
}
