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

    private void OnEnable()
    {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        SoundManager.instance.Play("Rainning", fadeTime);
        Debug.Log("Start sound");
        yield return new WaitForSeconds(duration);
        SoundManager.instance.StopPlay("Rainning", fadeTime);
        ParticleSystem.EmissionModule emissionModule = rainSystem.emission;
        float originEmissionRate = emissionModule.rateOverTime.constantMax;
        for (float f = fadeTime; f > 0 ; f -= Time.deltaTime)
        {
            float newMax = originEmissionRate * f / fadeTime;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(0.0f, 10.0f);
            yield return null;
        }
        gameObject.SetActive(false);
        Debug.Log("Stop sound");
    }
}
