using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MiniGame
{
	List<int> buttonNumber = new List<int>();

	public List<Text> buttonsText;
	public List<GameObject> buttons;
	public Sprite unlockSprite;

	private bool isCorrect = false;
	private bool isDone = false;

	void Start()
	{
		for(int i = 0; i < buttons.Capacity; i++)
		{
			buttonNumber.Add(0);
		}
	}

	void Update()
	{
		CheckIfCorrect();
		if(isCorrect && !isDone){
			Debug.Log("Correct!");
			for(int i = 0; i < buttons.Capacity; i++){
                buttons[i].SetActive(false);
			}
			
			Done();

			isDone = true;
		}
	}

    public void PressButton(int num)
    {
        //轉盤聲
        SoundManager.instance.Play("SwitchLock");
        if (num >= 4 && num <= 7){
    		buttonNumber[num-4] = (buttonNumber[num-4] + 1) % 10;
    		buttonsText[num-4].text = (buttonNumber[num-4]).ToString();
    	}
    	else if(num >= 8 && num <= 11){
    		buttonNumber[num-8] = (buttonNumber[num-8] + 9) % 10;
    		buttonsText[num-8].text = (buttonNumber[num-8]).ToString();
    	}
    }

    private void CheckIfCorrect()
    {
    	isCorrect = true;
    	for(int i = 0; i < password.Capacity; i++)
    	{
    		if(buttonNumber[i] != int.Parse(password[i])){
    			isCorrect = false;
    			break;
    		}
    	}
    }

    protected override void Done()
    {
        //開鎖聲
        SoundManager.instance.Play("Unlock");
        GetComponent<Image>().sprite = unlockSprite;

        base.Done();
    }
}
