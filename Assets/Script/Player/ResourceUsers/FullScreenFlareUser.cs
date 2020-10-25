﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenFlareUser : ResourceUser
{
    public GameObject flareInHand;

    protected override bool ItemAct()
    {
        if (flareInHand.activeSelf)
            return false;
        StartCoroutine(TriggerTheFlare());
        return true;
    }

    IEnumerator TriggerTheFlare()
    {
        yield return new WaitForSeconds(0.4f);
        flareInHand.SetActive(true);
    }
}
