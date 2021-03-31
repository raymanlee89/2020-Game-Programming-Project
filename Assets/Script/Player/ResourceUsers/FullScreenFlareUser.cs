using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenFlareUser : ResourceUser
{
    public GameObject flareInHand;

    protected override bool ItemAct()
    {
        StartCoroutine(TriggerTheFlare());
        UserControler.instance?.frashlightUser.TurnOffFrashlight();
        return true;
    }

    IEnumerator TriggerTheFlare()
    {
        yield return new WaitForSeconds(0.5f);
        flareInHand.SetActive(true);
    }
}
