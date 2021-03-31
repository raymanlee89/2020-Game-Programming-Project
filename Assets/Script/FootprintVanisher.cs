using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintVanisher : MonoBehaviour
{
    public float keepingTime;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(FootprintVanishing());
    }

    IEnumerator FootprintVanishing()
    {
        Color c = rend.material.color;
        float originalA = c.a;
        for (float f = keepingTime; f > 0 ; f -= Time.deltaTime)
        {
            rend.material.color = new Color(c.r, c.g, c.b, originalA * (f / keepingTime));
            yield return null;
        }
        Destroy(gameObject);
    }
}
