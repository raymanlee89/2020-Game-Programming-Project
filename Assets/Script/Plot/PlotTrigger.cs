using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlotTrigger : MonoBehaviour
{
    public DialogueTrigger plot = null;
    public DialogueTrigger plot2 = null;
    public string hint = "";
    public int hintChapterIndex;
    public TimelineAsset cutscene = null;
    public bool isTriggeredByCollider = true;
    public List<GameObject> openObjects;
    public List<GameObject> closeObjects;
    public bool turnOffPowerTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && isTriggeredByCollider)
        {
            Destroy(GetComponent<Collider2D>());
            TriggerPlot();
        }
    }

    public void TriggerPlot()
    {
        if (turnOffPowerTrigger)
            PlotManager.instance?.TurnOffPower();
        GameManager.instance.DisablePlayer();
        plot.TriggerDialogue();
        
        DialogueManager.instance.onDialogueEndCallBack += TriggerCutscene;
    }

    void TriggerCutscene() // if there is no cutscene, it will just show the hint
    {
        foreach (GameObject temp in openObjects)
        {
            temp.SetActive(true);
        }

        foreach (GameObject temp in closeObjects)
        {
            temp.SetActive(false);
        }
        Heartbeat.instance?.ScanEnemy();

        if (cutscene != null)
        {
            Debug.Log("Cutscene Start");
            TimelineManager.instance.StartCutscene(cutscene);
        }

        if (plot2 != null)
            TimelineManager.instance.playableDirector.stopped += TriggerPlot2;
        else
        {
            DialogueManager.instance.onDialogueEndCallBack -= TriggerCutscene;
            EndThePlot();
        }
    }

    void TriggerPlot2(PlayableDirector aDirector)
    {
        plot2.TriggerDialogue();
        DialogueManager.instance.onDialogueEndCallBack -= TriggerCutscene;
        TimelineManager.instance.playableDirector.stopped -= TriggerPlot2;
        DialogueManager.instance.onDialogueEndCallBack += EndThePlot;
    }

    void EndThePlot()
    {
        DialogueManager.instance.onDialogueEndCallBack -= EndThePlot;
        GameManager.instance.EnablePlayer();
        GameManager.instance.SaveGame();
        if (hint != "")
            PlotManager.instance.ShowHint(hint, hintChapterIndex);
        Destroy(gameObject);
    }
}
