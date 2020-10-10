using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintVanisher : MonoBehaviour
{
    public float keepingTime;
    public float fadingTime = 1f;
    float startTime = 0;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > keepingTime)
        {
            if(rend.material.color.a > 0)
            {
                Color c = rend.material.color;
                rend.material.color = new Color(c.r, c.g, c.b, c.a - ((1 / fadingTime) * Time.deltaTime));
                //Debug.Log(rend.material.color.a);
            }
            else
            {
                Destroy(gameObject);
                //Debug.Log("destroy");
            }
        }
    }
}
