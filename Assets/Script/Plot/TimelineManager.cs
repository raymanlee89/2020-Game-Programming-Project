using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;

    #region Singleton
    public static TimelineManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of TimelineManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playableDirector.enabled = false;
    }

    public void StartCutscene(TimelineAsset cutscene)
    {
        playableDirector.enabled = true;
        playableDirector.playableAsset = cutscene;
        playableDirector.Play();
    }
}
