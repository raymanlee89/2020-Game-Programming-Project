using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTrigger : MonoBehaviour
{
    // Trigger On enable
    // Stop With DurationTime

    public float duration;
    public float fadeTime;
    public ParticleSystem rainSystem;
    float originEmissionRate = 1000;

    IEnumerator coroutine = null;
    private void OnEnable()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = PlaySound();
        StartCoroutine(coroutine);
    }

    IEnumerator PlaySound()
    {
        ParticleSystem.EmissionModule emissionModule = rainSystem.emission;
        emissionModule.rateOverTime = originEmissionRate;
        SoundManager.instance.Play("Rainning", fadeTime);
        Debug.Log("Start sound");

        yield return new WaitForSeconds(duration);

        SoundManager.instance.StopPlay("Rainning", fadeTime);
        for (float f = fadeTime; f > 0 ; f -= Time.deltaTime)
        {
            float newMax = originEmissionRate * f / fadeTime;
            emissionModule.rateOverTime = newMax;
            yield return null;
        }
        gameObject.SetActive(false);
        Debug.Log("Stop sound");
    }
}
