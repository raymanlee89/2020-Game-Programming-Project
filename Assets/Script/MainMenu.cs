using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoScreen;
    float videoFadeDuration = 1.5f;

    private void Start()
    {
        SoundManager.instance?.Play("Rainning", 2f);
        SoundManager.instance?.Play("MainBGM", 2f);
        videoScreen.SetActive(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("NextSentence") && videoScreen.activeSelf)
        {
            LoadTheMainScene(videoPlayer);
        }
    }

    public void StartToPlayGame(string sceneName)
    {
        Debug.Log("Start Game");
        videoScreen.SetActive(true);
        StartCoroutine(FadeVideoScreen(sceneName));
    }

    IEnumerator FadeVideoScreen(string sceneName)
    {
        videoScreen.SetActive(true);
        RawImage image = videoScreen.GetComponent<RawImage>();
        image.color = new Color(0, 0, 0, 0);
        float newA = 0;
        while (newA < 1)
        {
            newA += Time.deltaTime / videoFadeDuration;
            image.color = new Color(0, 0, 0, newA);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 1);

        if(sceneName == "EazyScene")
            videoPlayer.loopPointReached += LoadTheEazyScene;
        else if (sceneName == "MainScene")
            videoPlayer.loopPointReached += LoadTheMainScene;
        videoPlayer.Play();
        float newC = 0;
        while (newC < 1)
        {
            newC += Time.deltaTime / videoFadeDuration;
            image.color = new Color(newC, newC, newC, 1);
            yield return null;
        }
        image.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(22f);
        SoundManager.instance?.StopPlay("Rainning", 5f);
        SoundManager.instance?.StopPlay("MainBGM", 5f);
    }

    void LoadTheMainScene(VideoPlayer videoPlayer)
    {
        videoPlayer.loopPointReached -= LoadTheMainScene;
        SceneChanger.instance?.FadeToMainScene();
    }

    void LoadTheEazyScene(VideoPlayer videoPlayer)
    {
        videoPlayer.loopPointReached -= LoadTheEazyScene;
        SceneChanger.instance?.FadeToEazyScene();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
