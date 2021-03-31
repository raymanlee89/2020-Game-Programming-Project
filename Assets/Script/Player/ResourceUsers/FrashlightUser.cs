﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrashlightUser : MonoBehaviour
{
    public GameObject frashlight;
    public float maxPower;
    public float flashingTimePeriod;
    public Item resource;
    public Item frashlightItem;
    float power;
    bool flashingOrNot = false;
    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        frashlight.SetActive(false);
        power = maxPower;
        UIManager.instance?.powerBar.SetMaxFill(maxPower);
        UIManager.instance?.powerBar.SetFill(power);
        UIManager.instance?.UpdatePowerBarNumber((power / maxPower * 100).ToString("0"));
    }

    private void OnEnable()
    {
        UIManager.instance?.OpenFrashlightUI(resource);
    }

    void Update()
    {
        if (GameManager.instance.IsPlayerDisable())
            return;

        if (Input.GetButtonDown("OpenFrashlight") && !flashingOrNot && !AnimationManager.instance.flareIsInHand && !UIManager.instance.mapPanel.activeSelf)
        {
            MainAction();
        }
        else if (frashlight.activeSelf && !flashingOrNot)// use power
        {
            if(power > 0)
            {
                power -= Time.deltaTime;
                UIManager.instance?.powerBar.SetFill(power);
                UIManager.instance?.UpdatePowerBarNumber((power / maxPower * 100).ToString("0"));
            }
            else // run out of power
            {
                StartCoroutine(Flashing());
                power = 0;
                UIManager.instance?.powerBar.SetFill(power);
                UIManager.instance?.UpdatePowerBarNumber("0");
            }
        }
        else if(power <= 0 && HasEnoughResource() && !frashlight.activeSelf && !flashingOrNot) // atomatically change battery
        {
            ChangeBattery();
        }
    }

    protected bool HasEnoughResource()
    {
        if (Inventory.instance == null)
            return false;
        return Inventory.instance.ContainResource(resource);
    }

    // switch the frashlight
    protected bool MainAction()
    {
        SoundManager.instance?.Play("ClickFrashlightSwitch");
        if(frashlight.activeSelf)
        {
            TurnOffFrashlight();
        }
        else if (power > 0)
        {
            TurnOnFrashlight();
        }
        return false;
    }

    public void TurnOnFrashlight()
    {
        StartCoroutine(TakeOutFrashlight());
    }

    IEnumerator TakeOutFrashlight()
    {
        AnimationManager.instance?.SwitchFrashlight(true);
        yield return new WaitForSeconds(0.2f);
        frashlight.SetActive(true);
    }

    public void TurnOffFrashlight()
    {
        frashlight.SetActive(false);
        AnimationManager.instance?.SwitchFrashlight(false);
    }

    // change the battery
    void ChangeBattery()
    {
        SoundManager.instance?.Play("ChangeBattery");
        RecoverPower();
        if (HasEnoughResource())
            inventory.Remove(resource);
    }

    public void RecoverPower()
    {
        power = maxPower;
        UIManager.instance?.powerBar.SetFill(power);
        UIManager.instance?.UpdatePowerBarNumber((power / maxPower * 100).ToString("0"));
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
        TurnOffFrashlight();
    }
}
