using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

	public static GameManager instance;

	void Awake(){

        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of GameManager found!");
            return;
        }
        instance = this;
	}

    #endregion

    public GameObject player;
    [HideInInspector]
    public bool playerIsInSafeAreaOrNot = false;

    int stopTimeCount = 0;
    int disablePlayerCount = 0;

    // player condition
    float[] playerPosition = new float[3];

    // inventory condition
    Dictionary<ItemData, int> resourceCount;
    List<ItemData> clues;

    // enviroment condition
    Pickable[] pickableItems;
    Openable[] doors;
    bool[] isDoorOpened = null;
    Playable[] miniGames;
    public GameObject rainTrigger;
    bool rainIsOn = false;

    public void DisablePlayer()
    {
        Debug.Log("Disable Player");
        disablePlayerCount++;
        if(disablePlayerCount == 1)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<UserControler>().enabled = false;
        }
    }

    public void EnablePlayer()
    {
        Debug.Log("Enable Player");
        disablePlayerCount--;
        if(disablePlayerCount == 0)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<UserControler>().enabled = true;
        }
        else if (disablePlayerCount < 0)
            disablePlayerCount = 0;
    }

    public bool IsPlayerDisable()
    {
        return disablePlayerCount > 0;
    }

    public void StopTime()
    {
        Debug.Log("Stop Time");
        stopTimeCount++;
        if (stopTimeCount == 1)
        {
            //SoundManager.instance?.PauseAllSound();
            Heartbeat heartBeats = FindObjectOfType<Heartbeat>();
            heartBeats?.Pause();
            Time.timeScale = 0f;
            DisablePlayer();
        }
    }

    public void RestartTime()
    {
        Debug.Log("Restart Time");
        stopTimeCount--;
        if (stopTimeCount == 0)
        {
            //SoundManager.instance?.UnPauseAllSound();
            Heartbeat heartBeats = FindObjectOfType<Heartbeat>();
            heartBeats?.UnPause();
            Time.timeScale = 1f;
            EnablePlayer();
        }
        else if (stopTimeCount < 0)
            stopTimeCount = 0;
    }

    public void SaveGame()
    {
        // player condition
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;

        // inventory condition
        resourceCount = Inventory.instance.GetResourceCount();
        clues = Inventory.instance.GetCluesList();

        // enviroment condition
        pickableItems = FindObjectsOfType<Pickable>();
        doors = FindObjectsOfType<Openable>();
        if (isDoorOpened == null)
            isDoorOpened = new bool[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            isDoorOpened[i] = doors[i].isOpen;
        }
        rainIsOn = rainTrigger.activeSelf;
        miniGames = FindObjectsOfType<Playable>();
    }

    public void LoadGame()
    {
        // player condition
        player.transform.position = new Vector3(playerPosition[0], playerPosition[1], playerPosition[2]);
        player.GetComponent<PlayerHealth>().RecoverHealth();
        player.GetComponent<PlayerMovement>().RecoverEnergy();
        player.GetComponent<FrashlightUser>().TurnOffFrashlight();
        player.GetComponent<FrashlightUser>().RecoverPower();

        // inventory condition
        Inventory.instance.ResetData(resourceCount, clues);

        // enviroment condition
        foreach (Pickable item in pickableItems)
        {
            item.gameObject.SetActive(true);
        }
        Pickable[]  tempPickableItems = FindObjectsOfType<Pickable>();
        foreach(Pickable item in tempPickableItems)
        {
            if(!Array.Exists(pickableItems, element => element == item))
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Reset(isDoorOpened[i]);
        }
        rainTrigger.SetActive(false);
        rainTrigger.SetActive(rainTrigger);

        foreach (Playable miniGame in miniGames)
        {
            miniGame.enabled = true;
        }

        StartCoroutine(RespawnWaiting());
    }

    IEnumerator RespawnWaiting()
    {
        UIManager.instance.ClossLossPanel();
        UIManager.instance.OpenLoadingPanel();

        yield return new WaitForSeconds(10f);

        UIManager.instance.ClossLoadingPanel();
        EnablePlayer();
    }
}
