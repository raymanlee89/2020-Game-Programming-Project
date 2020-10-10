using UnityEngine;

public class GlowStickUser: ResourceUser
{
    public GameObject glowStick;

    // drop a flare on the ground
    protected override bool ItemAct()
    {
        Instantiate(glowStick, transform.position, transform.rotation);
        return true;
    }
}
