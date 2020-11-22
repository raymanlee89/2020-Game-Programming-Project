using System.Collections;
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
        StartCoroutine(WaitHoldTheBandage());
        return true;
    }

    IEnumerator WaitHoldTheBandage()
    {
        yield return new WaitForSeconds(0.2f);
        playerHealth.Healing(playerHealth.maxHealth * healingPercentage);
        SoundManager.instance?.Play("Healing");
    }
}
