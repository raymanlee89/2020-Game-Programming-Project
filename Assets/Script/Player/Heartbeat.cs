using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography; //應該不用
using UnityEngine;
using UnityEngine.Audio;
using System;

public class Heartbeat : MonoBehaviour
{
    public int startingPitch = 1; //初始化pitch
    AudioSource audioSource; 
    
    public float minDist = 0.5f;
    public float maxDist = 6f;
    public float closePitch = 1.5f; //pitch會被audiomixer凹回來，所以這邊是速度
    public float farPitch = 0.5f;
    public AudioMixer AudioSpeedUp;
    GameObject[] enemies = null;
    GameObject closest = null;

    #region Singleton

    public static Heartbeat instance;

    void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of Heartbeat found!");
            return;
        }
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the AudioSource from the GameObject
        audioSource = GetComponent<AudioSource>();

        //Initialize the pitch
        audioSource.pitch = startingPitch;

        audioSource.volume = 0; //調音量

        ScanEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Length == 0)
            return;

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
        audioSource.volume = Mathf.Pow((maxDist - x) / maxDist, 3); //調音量
        AudioSpeedUp.SetFloat("pitchBend", 1f / pitch); //讓mixer調回去
    }

    public void ScanEnemy()
    {
        // go = game object, gos是複數個go的array
        Debug.Log("Scan enemies");
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //找所有帶有"enemy" tag的物件
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDist);
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
    }
}
