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
        playerMovement.RecoverEnergy();
        SoundManager.instance?.Play("DrinkWater");
        return true;
    }
}
