using System.Collections;
using UnityEngine;

public class GlowStickUser: ResourceUser
{
    public GameObject glowStick;
    public Transform fallPoint;

    // drop a flare on the ground
    protected override bool ItemAct()
    {
        StartCoroutine(WaitTheStickFall());
        return true;
    }

    IEnumerator WaitTheStickFall()
    {
        yield return new WaitForSeconds(0.3f);
        SoundManager.instance?.Play("DropGlowStick");
        Instantiate(glowStick, fallPoint.position, fallPoint.rotation);
    }
}
