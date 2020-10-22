using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flare : MonoBehaviour
{
    public float flareKeepingTime;
    public float flareMaxInstanseTime;
    public Light2D flareLight;

    void Start()
    {
        gameObject.SetActive(false);
    }

    IEnumerator BurnTheFlare()
    {
        SoundManager.instance?.Play("FlareBurnning");
        flareLight.intensity = 0;
        for (float f = 0; f < flareMaxInstanseTime; f += Time.deltaTime)
        {
            flareLight.intensity = f / flareMaxInstanseTime;
            yield return null;
        }
        for (float f = flareKeepingTime; f > 0; f -= Time.deltaTime)
        {
            flareLight.intensity = f / flareKeepingTime;
            yield return null;
        }
        flareLight.intensity = 0;
        SoundManager.instance?.StopPlay("FlareBurnning", 1f);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(BurnTheFlare());
    }
}
