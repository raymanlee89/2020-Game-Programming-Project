using System.Collections;
using UnityEngine;

public class WaterUser : ResourceUser
{
    protected override bool ItemAct()
    {
        if (PlayerMovement.instance.IsFullEnergy())
            return false;
        StartCoroutine(WaitHoldTheBottle());
        return true;
    }

    IEnumerator WaitHoldTheBottle()
    {
        yield return new WaitForSeconds(0.3f);
        SoundManager.instance?.Play("DrinkWater");
        PlayerMovement.instance?.RecoverEnergy();
    }
}
