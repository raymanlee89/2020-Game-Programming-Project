using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography; //應該不用
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundSpeed : MonoBehaviour
{
    public int startingPitch = 1; //初始化pitch
    AudioSource audioSource; 
    
    public float minDist = 0.5f;
    public float maxDist = 6f;
    public float closePitch = 1.5f; //pitch會被audiomixer凹回來，所以這邊是速度
    public float farPitch = 0.5f;
    public AudioMixer AudioSpeedUp;
    GameObject[] enemies;
    GameObject closest = null;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the AudioSource from the GameObject
        audioSource = GetComponent<AudioSource>();

        //Initialize the pitch
        audioSource.pitch = startingPitch;

        enemies = ScanEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity; //一開始為無限遠（等於沒enemy）
        Vector3 position = transform.position;
        foreach (GameObject go in enemies) //對每個找
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go; //最近
                distance = curDistance;
            }
        }

        //Replace your if/else/elseif with this:
        float dist = Vector2.Distance(closest.transform.position, transform.position);
        float x = Mathf.Clamp(dist, minDist, maxDist);
        float pitch = (farPitch - closePitch) * (x - minDist) / (maxDist - minDist) + closePitch;
        audioSource.pitch = pitch;
        audioSource.volume = pitch-0.5f; //調音量
        AudioSpeedUp.SetFloat("pitchBend", 1f / pitch); //讓mixer調回去
    }

    public GameObject[] ScanEnemy()
    {
        // go = game object, gos是複數個go的array
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy"); //找所有帶有"enemy" tag的物件
        return gos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDist);
    }
}
