using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    protected List<string> password;
    protected Playable presentOwner = null;
    public GameObject panel;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    public void SetPassword(List<string> newPassword)
    {
        password = newPassword;
    }

    protected virtual void Done()
    {
        StartCoroutine(WaitToSendGift());
        GetComponent<Image>().sprite = unlockSprite;
    }

    public void SetOwner(Playable newOwner)
    {
        Reset();
        presentOwner = newOwner;
    }

    protected virtual IEnumerator WaitToSendGift()
    {
        yield return new WaitForSeconds(1f);
        presentOwner?.SendGifts();
        panel.SetActive(false);
        Reset();
    }

    protected virtual void Reset()
    {
        presentOwner = null;
        GetComponent<Image>().sprite = lockSprite;
    }

    public void CloseMiniGame()
    {
        presentOwner?.PausePlay();
    }
}
