using System.Collections;
using UnityEngine;

public class WaterUser : ResourceUser
{
    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override bool ItemAct()
    {
        if (playerMovement.IsFullEnergy())
            return false;
        StartCoroutine(WaitHoldTheBottle());
        return true;
    }

    IEnumerator WaitHoldTheBottle()
    {
        yield return new WaitForSeconds(0.3f);
        SoundManager.instance?.Play("DrinkWater");
        playerMovement.RecoverEnergy();
    }
}
