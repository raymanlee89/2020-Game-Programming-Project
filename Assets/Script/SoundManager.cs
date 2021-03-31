using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float maxDistence;
    string presentBGM = "";
    public Sound[] sounds;
    List<string> pausedList = new List<string>();

    public static SoundManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.isFadingOutOrNot = false;
        }
    }

    public void Play(string name, float fadeTime = 0, Transform soundCreator = null)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.isFadingOutOrNot = false;
        if (soundCreator != null)
        {
            float dis = Vector2.Distance(transform.position, soundCreator.position);
            if (dis > maxDistence)
                return;
            else
                s.source.volume = s.volume * ((maxDistence - dis) / maxDistence);
        }

        s.source.Play();
        if(fadeTime != 0)
            StartCoroutine(FadeIn(s, fadeTime));
    }

    IEnumerator FadeIn(Sound s, float fadeTime)
    {
        s.source.volume = 0;

        while (s.source.volume < s.volume)
        {
            s.source.volume += s.volume * Time.deltaTime / fadeTime;

            yield return null;
        }
        s.source.volume = s.volume;
    }

    public void StopPlay(string name, float fadeTime = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        else if (!s.source.isPlaying)
            return;

        if (fadeTime != 0)
            StartCoroutine(FadeOut(s, fadeTime));
        else
        {
            s.isFadingOutOrNot = false;
            s.source.Stop();
            s.source.volume = s.volume;
        }
    }

    IEnumerator FadeOut(Sound s, float fadeTime)
    {
        s.isFadingOutOrNot = true;
        while (s.source.volume > 0)
        {
            if (s.isFadingOutOrNot)
                s.source.volume -= s.volume * Time.deltaTime / fadeTime;
            else
            {
                s.source.volume = s.volume;
                yield break;
            }

            yield return null;
        }

        if (s.isFadingOutOrNot)
        {
            s.source.Stop();
            s.source.volume = s.volume;
        }
        else
        {
            s.source.volume = s.volume;
            yield break;
        }
    }

    public void PauseAllSound()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                s.source.Pause();
                pausedList.Add(s.name);
            }
        }
    }

    public void UnPauseAllSound()
    {
        foreach (Sound s in sounds)
        {
            if (pausedList.Contains(s.name))
                s.source.UnPause();
        }
        pausedList.Clear();
    }

    public void PlayBGM(string BGM)
    {
        if (BGM != presentBGM || presentBGM == "")
        {
            StopPlay(presentBGM, 2);
            Play(BGM);
            presentBGM = BGM;
        }
    }
}
