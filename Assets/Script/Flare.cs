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
        SoundManager.instance?.StopPlay("FlareBurnning");
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
        yield return new WaitForSeconds(flareKeepingTime - 3);
        for (float f = 3; f > 0; f -= Time.deltaTime)
        {
            flareLight.intensity = f / 3;
            yield return null;
        }
        flareLight.intensity = 0;
        SoundManager.instance?.StopPlay("FlareBurnning", 1f);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(BurnTheFlare());
        ChaseEnemy[] enemies = FindObjectsOfType<ChaseEnemy>();
        foreach(ChaseEnemy enemy in enemies)
        {
            enemy.SlowDown();
            Debug.Log("slow down enemy");
        }

        StandAndChaseEnemy[] enemies2 = FindObjectsOfType<StandAndChaseEnemy>();
        foreach (StandAndChaseEnemy enemy in enemies2)
        {
            enemy.SlowDown();
            Debug.Log("slow down enemy");
        }
    }
}
