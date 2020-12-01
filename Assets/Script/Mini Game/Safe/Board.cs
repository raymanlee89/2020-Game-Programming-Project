using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Board : MiniGame
{
    public Image dial;
    //light indicating correct password
    public Image light0;
    public Image light1;
    public Image light2;
    public Image light3;
    public int passLength = 4;
    private string[] inputValues;
    private int inputIndex;

    // Start is called before the first frame update
    void Start()
    {
        turning safeDial = dial.GetComponent<turning>();
        safeDial.dialInput = DialInput;
        InitGame();
    }

    public void InitGame()
    {
        inputIndex = 0;
        inputValues = new string[passLength];
        
        light0.color = Color.red;
        light1.color = Color.red;
        light2.color = Color.red;
        light3.color = Color.red;
    }

    IEnumerator WaitSomeTime()
    {
        //Print the time of when the function is first called.
        yield return new WaitForSeconds(2);
        light0.color = Color.red;
        light1.color = Color.red;
        light2.color = Color.red;
        light3.color = Color.red;
        //reset input values
        inputValues[0] = "R0";
        inputValues[1] = "R0";
        inputValues[2] = "R0";
        inputValues[3] = "R0";
        //After we have waited 2 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    protected override void Done()
    {
        dial.GetComponent<turning>().enabled = false;
        Debug.Log("Game Over");

        base.Done();
    }

    private void DialInput(string value)
    {
        inputValues[inputIndex] = value;
        bool gameover = false;
        
        inputIndex = (inputIndex + 1 /* + passLength*/) % passLength;
        //see current input value
        Debug.Log("result is " + inputValues[0] + inputValues[1] + inputValues[2] + inputValues[3]  + " input index is " + inputIndex);

        if (inputValues[0] == password[0] && inputValues[1] == password[1] && inputValues[2] == password[2] && inputValues[3] == password[3])
        {
            //when all correct
            light0.color = Color.green;
            light1.color = Color.green;
            light2.color = Color.green;
            light3.color = Color.green;
            gameover = true;

            //call game over function for returning to main game
            Done();
        }
        //first correct light up and play sound
        if(inputValues[0] == password[0])
        {
            
            if(inputIndex -1 == 0)
            {
                //GetComponent<AudioSource>().Play();
                light0.GetComponent<Image>().color = Color.green;
            }
            
            //Debug.Log("first one ok");
        }
        else
        {
            light0.GetComponent<Image>().color = Color.red;
        }
        //second correct light up and play sound
        if (inputValues[1] == password[1])
        {
            if (inputIndex -1 == 1)
            {
                //GetComponent<AudioSource>().Play();
                light1.GetComponent<Image>().color = Color.green;
            }
            //Debug.Log("second one ok");
        }
        else
        {
           light1.GetComponent<Image>().color = Color.red;
        }
        //third correct light up and play sound
        if (inputValues[2] == password[2])
        {
            
            if (inputIndex -1 == 2)
            {
                //GetComponent<AudioSource>().Play();
                light2.GetComponent<Image>().color = Color.green;
            }
            //Debug.Log("third one ok");
        }
        else
        {
            light2.GetComponent<Image>().color = Color.red;
        }
        //fourth correct light up and play sound
        if (inputValues[3] == password[3])
        {
            
            if (inputIndex +3 == 3)
            {
                //GetComponent<AudioSource>().Play();
                light3.GetComponent<Image>().color = Color.green;
            }
            //Debug.Log("fourth one ok");
        }
        else
        {
            light3.GetComponent<Image>().color = Color.red;
        }

        if(inputIndex == 0 && !gameover)
        {
            //wait 2 sec and reset light and input
            StartCoroutine(WaitSomeTime());
        }
    }

}
