using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrashlightUser : MonoBehaviour
{
    public GameObject frashlight;
    public float maxPower;
    public Bar powerBar;
    public float flashingTimePeriod;
    public Text powerBarNumber;
    public Item resource;
    float power;
    bool flashingOrNot = false;
    Inventory inventory;
    
    void Start()
    {
        inventory = Inventory.instance;
        frashlight.SetActive(false);
        power = 0;
        powerBar.SetMaxFill(maxPower);
        powerBar.SetFill(power);
        powerBarNumber.text = (power / maxPower * 100).ToString("0") + "%";
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (Input.GetButtonDown("OpenFrashlight") && !flashingOrNot)
        {
            MainAction();
        }
        else if (frashlight.activeSelf && !flashingOrNot)// use power
        {
            if(power > 0)
            {
                power -= Time.deltaTime;
                powerBar.SetFill(power);
                powerBarNumber.text = (power / maxPower * 100).ToString("0") + "%";
            }
            else // run out of power
            {
                StartCoroutine(Flashing());
                power = 0;
                powerBar.SetFill(power);
                powerBarNumber.text = (power / maxPower * 100).ToString("0") + "%";
            }
        }
        else if(power <= 0 && HasEnoughResource() && !frashlight.activeSelf && !flashingOrNot) // atomatically change battery
        {
            ChangeBattery();
        }
    }

    protected bool HasEnoughResource()
    {
        if (inventory.resourceCount.ContainsKey(resource))
        {
            if (inventory.resourceCount[resource] > 0)
                return true;
        }
        return false;
    }

    // switch the frashlight
    protected bool MainAction()
    {
        SoundManager.instance?.Play("ClickFrashlightSwitch");
        if(frashlight.activeSelf)
        {
            frashlight.SetActive(false);
        }
        else if (power > 0)
        {
            frashlight.SetActive(true);
        }
        return false;
    }

    // change the battery
    void ChangeBattery()
    {
        SoundManager.instance?.Play("ChangeBattery");
        power = maxPower;
        powerBar.SetFill(power);
        powerBarNumber.text = (power / maxPower * 100).ToString("0") + "%";
        if(HasEnoughResource())
            inventory.Remove(resource);
    }

    IEnumerator Flashing()
    {
        //Debug.Log("Start flashing");
        flashingOrNot = true;
        for(int i=0; i<2; i++) // flashs twice
        {
            frashlight.SetActive(false);
            yield return new WaitForSeconds(flashingTimePeriod);
            frashlight.SetActive(true);
            yield return new WaitForSeconds(flashingTimePeriod);
        }
        frashlight.SetActive(false);
        flashingOrNot = false;
    }
}
