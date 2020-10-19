using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flare : MonoBehaviour
{
    public float flareKeepingTime;
    public Light2D flareLight;
    float flareKeepingTimeLeft;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        flareKeepingTimeLeft -= Time.deltaTime;
        if (flareKeepingTimeLeft > 0)
            flareLight.intensity = flareKeepingTimeLeft / flareKeepingTime;
        else
        {
            SoundManager.instance?.StopPlay("FlareBurnning", 1f);
            gameObject.SetActive(false);
        }
            
    }

    void OnEnable()
    {
        SoundManager.instance?.Play("FlareBurnning");
        flareLight.intensity = 1;
        flareKeepingTimeLeft = flareKeepingTime;
    }
}
