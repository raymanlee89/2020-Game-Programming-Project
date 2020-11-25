using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject dial;
    //light indicating correct password
    public Image light0;
    public Image light1;
    public Image light2;
    public Image light3;
    public int passLength = 4;
    private string[] passCombination;
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
        dial.GetComponent<CircleCollider2D>().enabled = true; //make it turnable
        inputIndex = 0;
        inputValues = new string[passLength];
        passCombination = new string[passLength];
        passCombination[0] = "R60";
        passCombination[1] = "L30";
        passCombination[2] = "R80";
        passCombination[3] = "L60";
        //generate random pass word (not used)
        /*for (int i =0; i < passLength; ++i)
        {
            int direction = (1 - (Random.Range(0, 2) * 2));
            int passValue = Random.Range(0, 10);
            
            passCombination += (direction > 0 ? "R" : "L") + (passValue * 10).ToString();

        }*/
        Debug.Log("password is " + passCombination[0] + passCombination[1] + passCombination[2] + passCombination[3]);
        //default color red (wrong)
        light0.color = Color.red;
        light1.color = Color.red;
        light2.color = Color.red;
        light3.color = Color.red;
    }

    IEnumerator WaitSomeTime()
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 2 seconds.
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

    private void DialInput(string value)
    {
        inputValues[inputIndex] = value;
        bool gameover = false;
        
        inputIndex = (inputIndex + 1 /* + passLength*/) % passLength;
        //see current input value
        Debug.Log("result is " + inputValues[0] + inputValues[1] + inputValues[2] + inputValues[3]  + " input index is " + inputIndex);

        if (inputValues[0] == passCombination[0] && inputValues[1] == passCombination[1] && inputValues[2] == passCombination[2] && inputValues[3] == passCombination[3])
        {
            //when all correct
            light0.color = Color.green;
            light1.color = Color.green;
            light2.color = Color.green;
            light3.color = Color.green;
            gameover = true;
            Debug.Log("Game Over");
            //disable turning
            dial.GetComponent<CircleCollider2D>().enabled = false;
        }
        //first correct light up and play sound
        if(inputValues[0] == passCombination[0])
        {
            //Get the Renderer component from the new cube
            var lightRenderer = light0.GetComponent<Renderer>();

            if(inputIndex -1 == 0)
            {
                GetComponent<AudioSource>().Play();
                lightRenderer.material.color = Color.green;
            }
            
            //Debug.Log("first one ok");
        }
        else
        {
            var lightRenderer = light0.GetComponent<Renderer>();
            lightRenderer.material.color = Color.red;
        }
        //second correct light up and play sound
        if (inputValues[1] == passCombination[1])
        {
            //Get the Renderer component from the new cube
            var lightRenderer = light1.GetComponent<Renderer>();

            if (inputIndex -1 == 1)
            {
                GetComponent<AudioSource>().Play();
                lightRenderer.material.color = Color.green;
            }
            //Debug.Log("second one ok");
        }
        else
        {
            var lightRenderer = light1.GetComponent<Renderer>();
            lightRenderer.material.color = Color.red;
        }
        //third correct light up and play sound
        if (inputValues[2] == passCombination[2])
        {
            //Get the Renderer component from the new cube
            var lightRenderer = light2.GetComponent<Renderer>();

            if (inputIndex -1 == 2)
            {
                GetComponent<AudioSource>().Play();
                lightRenderer.material.color = Color.green;
            }
            //Debug.Log("third one ok");
        }
        else
        {
            var lightRenderer = light2.GetComponent<Renderer>();
            lightRenderer.material.color = Color.red;
        }
        //fourth correct light up and play sound
        if (inputValues[3] == passCombination[3])
        {
            //Get the Renderer component from the new cube
            var lightRenderer = light3.GetComponent<Renderer>();

            if (inputIndex +3 == 3)
            {
                GetComponent<AudioSource>().Play();
                lightRenderer.material.color = Color.green;
            }
            //Debug.Log("fourth one ok");
        }
        else
        {
            var lightRenderer = light3.GetComponent<Renderer>();
            lightRenderer.material.color = Color.red;
        }

        if(inputIndex == 0 && !gameover)
        {
            //wait 2 sec and reset light and input
            StartCoroutine(WaitSomeTime());
        }
    }
}
