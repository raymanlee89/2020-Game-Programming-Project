using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using SAP2D;

public class Instructor : ChaseEnemy
{
    public SpriteRenderer GFX;
    public float GFXVanishingTime;
    public GameObject nextInstructor;
    public PlotTrigger changeStagePlot;
    public GameObject shinning;
    public GameObject suicideNote;
    ShadowCaster2D shadow;

    public void ChangeStage()
    {
        Debug.Log("Change stage");
        DialogueManager.instance.onDialogueEndCallBack += OpenNextInstructor;
        Disappear();
    }

    void OpenNextInstructor()
    {
        if (nextInstructor != null)
            nextInstructor.SetActive(true);
        else
        {
            if(suicideNote != null)
            {
                suicideNote.SetActive(true);
                suicideNote.transform.position = transform.position;
            }
        }
            
        GameManager.instance.EnablePlayer();
        DialogueManager.instance.onDialogueEndCallBack -= OpenNextInstructor;
        gameObject.SetActive(false);
    }

    protected override void Disappear()
    {
        Debug.Log("Disappear");
        GameManager.instance.DisablePlayer();
        agent.enabled = false;
        state = EnemyState.Unactive;
        gameObject.tag = "Untagged";
        shadow = GetComponent<ShadowCaster2D>();
        if (shadow != null)
            shadow.enabled = false;
        Heartbeat.instance?.ScanEnemy();
        GameManager.instance?.LosePlayer();
        StartCoroutine(GFXVanishing());
    }

    IEnumerator GFXVanishing()
    {
        shinning.SetActive(true);
        Color c = GFX.color;

        GFX.color = new Color(c.r, c.g, c.b, 0);

        for (float f = 0; f < GFXVanishingTime; f += Time.deltaTime)
        {
            GFX.color = new Color(c.r, c.g, c.b, f / GFXVanishingTime);
            yield return null;
        }

        GFX.material.color = new Color(c.r, c.g, c.b, 1);

        for (float f = GFXVanishingTime; f > 0; f -= Time.deltaTime)
        {
            GFX.color = new Color(c.r, c.g, c.b, f / GFXVanishingTime);
            yield return null;
        }

        if (changeStagePlot != null)
            changeStagePlot.TriggerPlot();
        else
        {
            OpenNextInstructor();
        }
    }
}
