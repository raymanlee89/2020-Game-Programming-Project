using UnityEngine;

public class WaterUser : ResourceUser
{
    public GameObject puddle;

    protected override bool ItemAct()
    {
        Instantiate(puddle, transform.position, transform.rotation);
        return true;
    }
}
