using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [TextArea(1, 10)]
    public List<string> chapterMarks;
    int presentChapterIndex = -1;

    public bool powerIsOnOrNot = false;

    void ChangeChapter(int chapterIndex)
    {
        presentChapterIndex = chapterIndex;
        UIManager.instance.UpdateChapterText(chapterMarks[chapterIndex]);
    }

    public void ShowHint(string hint, int chapterIndex)
    {
        UIManager.instance?.UpdateHintText(hint);
        if (chapterIndex != presentChapterIndex)
            ChangeChapter(chapterIndex);
    }

    public void TurnOnPower()
    {
        powerIsOnOrNot = true;
    }
}
