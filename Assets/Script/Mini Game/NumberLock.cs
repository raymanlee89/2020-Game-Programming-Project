using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberLock : MonoBehaviour
{
    public GameObject panel;
    List<string> password;
    public GameObject buttons;
	public Image LightButton;
	public List<Sprite> LightSprite;
	List<int> digitsNumber = new List<int>();
	int current_digit;
	bool isCorrect = false;
    ElectronicOpenable presentOwner = null;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < password.Capacity; i++)
        {
        	digitsNumber.Add(10);
        }
        current_digit = 0;
    }

    public void Pressbutton(int num)
    {
        SoundManager.instance?.Play("Beep");
    	if(current_digit >= password.Capacity)
    	{
    		if(num == 10)
    		{
    			CheckIfCorrect();
    		}
    		else if(num == 11)
    		{
    			Debug.Log("Clear");
    			StartCoroutine(InputLight());
    			TempReset();
    		}
    		else if(num <= 9 && num >= 0){
    			Debug.Log(current_digit);
    			StartCoroutine(InputLight());
    			current_digit = current_digit + 1;
    		}
    	}
    	else
    	{
			if(num == 11)
    		{
    			StartCoroutine(InputLight());
    			TempReset();
    			Debug.Log("Clear");
    		}
    		if(num == 10)
    		{
    			StartCoroutine(WrongLight());
    			TempReset();
    		}
    		else if(num <= 9 && num >= 0)
    		{
    			Debug.Log(current_digit);
    			StartCoroutine(InputLight());
		    	digitsNumber[current_digit] = num;
		    	current_digit = current_digit + 1;
		    }
    	}
    	
    }

    void CheckIfCorrect(){
    	isCorrect = true;
    	if(current_digit == password.Capacity)
    	{
    		for(int i = 0; i < password.Capacity; i++)
	    	{
	    		if(digitsNumber[i] != int.Parse(password[i]))
	    		{
	    			isCorrect = false;
	    			break;
	    		}
	    	}	    	
    	}
    	else
    	{
    		isCorrect = false;
    	}
    	if(isCorrect)
    	{
            //播放正確音效、開鎖聲
            SoundManager.instance?.Play("CorrectPassword");
    		StartCoroutine(CorrectLight());
    		Debug.Log("correct");
    	}
    	else
    	{
            //播放錯誤音效
            SoundManager.instance?.Play("WrongPassword");
            Debug.Log("Wrong!");
    		StartCoroutine(WrongLight());
    		TempReset();
    	}
    }

    void Done()
    {
        Debug.Log("Done");
        presentOwner?.Unlock();
        buttons.SetActive(false);
        Reset();
        panel.SetActive(false);
    }

    void TempReset()
    {
        current_digit = 0;
        for (int i = 0; i < password.Capacity; i++)
        {
            digitsNumber[i] = 10;
        }
    }

    private void Reset()
    {
        TempReset();

        // real reset
        buttons.SetActive(true);
        isCorrect = false;
        LightButton.sprite = null;
        presentOwner = null;
    }

    IEnumerator InputLight()
    {
    	LightButton.sprite = LightSprite[0];
    	yield return new WaitForSeconds(0.3f);
    	LightButton.sprite = null;
    }

    IEnumerator CorrectLight()
    {
    	LightButton.sprite = LightSprite[1];
        yield return new WaitForSeconds(1.0f);
        Done();
        LightButton.sprite = null;
    }

    IEnumerator WrongLight()
    {
    	LightButton.sprite = LightSprite[2];
        yield return new WaitForSeconds(1.0f);
    	LightButton.sprite = null;
    }

    public void SetPassword(List<string> newPassword)
    {
        password = newPassword;
    }

    public void SetOwner(ElectronicOpenable newOwner)
    {
        Debug.Log("SetOwner");
        presentOwner = newOwner;
    }

    public void CloseMiniGame()
    {
        presentOwner?.PausePlay();
    }
}
