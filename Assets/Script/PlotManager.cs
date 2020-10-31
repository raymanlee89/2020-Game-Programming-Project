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
    public List<string> hints;

    private void Start()
    {
        ShowHint(0);
    }

    public void ShowHint(int hintIndex)
    {
        UIManager.instance?.UpdateHintText(hints[hintIndex]);
    }
}
