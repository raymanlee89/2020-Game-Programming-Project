using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlotManager : MonoBehaviour
{
    #region Singleton

    public static PlotManager instance;

    void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of PlotManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    public VideoClip endingClip;
    public VideoClip staffClip;
    public VideoPlayer videoPlayer;

    public int EndChapterIndex = 3;
    [TextArea(1, 10)]
    public List<string> chapterMarks;
    int presentChapterIndex = -1;

    public bool powerIsOnOrNot;
    

    public delegate void OnPowerSwitched(bool powerIsOnOrNot);
    public OnPowerSwitched OnPowerSwitchedCallBack;

    private void Start()
    {
        powerIsOnOrNot = false;
    }

    void ChangeChapter(int chapterIndex)
    {
        presentChapterIndex = chapterIndex;
        UIManager.instance?.UpdateChapterText(chapterMarks[chapterIndex]);
        
        if (chapterIndex == EndChapterIndex)
            PlayEndingClip();
    }

    public void ShowHint(string hint, int chapterIndex)
    {
        UIManager.instance?.UpdateHintText(hint);
        if (chapterIndex != presentChapterIndex)
            ChangeChapter(chapterIndex);
    }

    public void TurnOnPower()
    {
        SoundManager.instance?.Play("ClickLightSwitch");
        powerIsOnOrNot = true;
        OnPowerSwitchedCallBack?.Invoke(powerIsOnOrNot);
    }

    public void TurnOffPower()
    {
        SoundManager.instance?.Play("ClickLightSwitch");
        powerIsOnOrNot = false;
        OnPowerSwitchedCallBack?.Invoke(powerIsOnOrNot);
    }

    void PlayEndingClip()
    {
        Debug.Log("Game is over");
        GameManager.instance?.DisablePlayer();
        videoPlayer.clip = endingClip;
        UIManager.instance?.OpenVideoScreen();
        videoPlayer.loopPointReached += PlayStaffClip;
        videoPlayer.Play();
    }

    void PlayStaffClip(VideoPlayer videoPlayer)
    {
        videoPlayer.loopPointReached -= PlayStaffClip;
        videoPlayer.clip = staffClip;
        videoPlayer.Play();
        videoPlayer.loopPointReached += ShowTrophy;
    }

    void ShowTrophy(VideoPlayer videoPlayer)
    {
        UIManager.instance?.CloseVideoScreen();
        UIManager.instance?.ShowTrophy();
        videoPlayer.loopPointReached -= ShowTrophy;
    }
}
