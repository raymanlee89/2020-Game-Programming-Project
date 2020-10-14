using UnityEngine;

public class GlowStickUser: ResourceUser
{
    public GameObject glowStick;

    // drop a flare on the ground
    protected override bool ItemAct()
    {
        SoundManager.instance?.Play("DropGlowStick");
        Instantiate(glowStick, transform.position, transform.rotation);
        return true;
    }
}
