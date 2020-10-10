using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenFlareUser : ResourceUser
{
    public GameObject flareInHand;

    protected override bool ItemAct()
    {
        flareInHand.SetActive(true);
        return true;
    }
}
