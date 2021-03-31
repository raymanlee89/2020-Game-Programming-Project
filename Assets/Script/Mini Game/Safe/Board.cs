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
    int passLength = 4;
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

    protected override void Done()
    {
        SoundManager.instance.Play("Unlock");

        base.Done();
    }

    private void DialInput(string value)
    {
        Debug.Log(inputIndex);
        inputValues[inputIndex] = value;
        
        //first correct light up and play sound
        if(inputValues[inputIndex] == password[inputIndex])
        {
            if(inputIndex == 0)
                light0.GetComponent<Image>().color = Color.green;
            if(inputIndex == 1)
                light1.GetComponent<Image>().color = Color.green;
            if (inputIndex == 2)
                light2.GetComponent<Image>().color = Color.green;
            if (inputIndex == 3)
            {
                light3.GetComponent<Image>().color = Color.green;
                Done();
            }
            inputIndex = (inputIndex + 1) % passLength;
        }
        else
        {
            TempReset();
        }
    }

    protected override void Reset()
    {
        if(presentOwner != null)
            TempReset();

        base.Reset();
    }

    public void TempReset()
    {
        inputIndex = 0;
        dial.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
        light0.color = Color.red;
        light1.color = Color.red;
        light2.color = Color.red;
        light3.color = Color.red;
        inputValues[0] = "R0";
        inputValues[1] = "R0";
        inputValues[2] = "R0";
        inputValues[3] = "R0";
    }
}
