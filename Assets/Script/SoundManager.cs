using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float maxDistence;
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
        }
    }

    public void Play(string name, float fadeTime = 0, Transform soundCreator = null)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
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
            StartCoroutine(FadeIn(s.source, fadeTime));
    }

    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeTime;

            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    public void StopPlay(string name, float fadeTime = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        if (fadeTime != 0)
            StartCoroutine(FadeOut(s.source, fadeTime));
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
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
}
