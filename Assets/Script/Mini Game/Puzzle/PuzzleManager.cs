using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MiniGame
{
    public Image PipesHolder;
    //public Image[] Pipes;
    [HideInInspector]
    public bool[] flow;
    public int totalPipes = 11;
    int correctedPipes = 0;
    [HideInInspector]
    public bool complete = false;
    
    void Awake()
    {
        //Pipes = new Image[totalPipes];
        flow = new bool[totalPipes];

        //initialize pipes[] and flow[]
        for (int i = 0; i < totalPipes; i++)
        {
            //Pipes[i] = PipesHolder.transform.GetChild(i).Image;
            flow[i] = false;
        }
    }

    private void OnEnable()
    {
        SoundManager.instance.Play("OpenDoor");
    }

    public void correctMove()
    {
        correctedPipes += 1;
        
        //Debug.Log("correct Move ");
        //if all blocks are correct
        if(correctedPipes == totalPipes)
        {
            //Debug.Log("You win!");
            Done();
            complete = true;
        }
    }

    public void wrongMove()
    {
        correctedPipes -= 1;
        //Debug.Log("wrong Move");
    }

    protected override void Done()
    {
        PlotManager.instance?.TurnOnPower();
        Debug.Log("Turn on power!!");
        GetComponent<Image>().sprite = unlockSprite;
        StartCoroutine(WaitToSendGift());
    }

    protected override IEnumerator WaitToSendGift()
    {
        yield return new WaitForSeconds(1f);
        SoundManager.instance?.Play("TurnOnPower");
        presentOwner?.SendGifts();
        panel.SetActive(false);
        Reset();
    }

    protected override void Reset()
    {
        presentOwner = null;
        GetComponent<Image>().sprite = lockSprite;
    }
}
