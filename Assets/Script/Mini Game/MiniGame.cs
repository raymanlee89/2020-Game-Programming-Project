using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    protected List<string> password;
    protected Playable presentOwner = null;
    public GameObject panel;

    public void SetPassword(List<string> newPassword)
    {
        password = newPassword;
    }

    protected virtual void Done()
    {
        StartCoroutine(WaitToSendGift());
    }

    public void SetOwner(Playable newOwner)
    {
        presentOwner = newOwner;
    }

    protected IEnumerator WaitToSendGift()
    {
        yield return new WaitForSeconds(1f);
        presentOwner?.SendGifts();
        panel.SetActive(false);
    }

    public void CloseMiniGame()
    {
        panel.SetActive(false);
        GameManager.instance.EnablePlayer();
    }
}
