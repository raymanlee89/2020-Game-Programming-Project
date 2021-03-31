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
    public bool playerIsInSafeAreaOrNot = false;
    public bool isPlayerAlive = true;

    int stopTimeCount = 0;
    int disablePlayerCount = 0;

    // player condition
    float[] playerPosition = new float[3];

    // inventory condition
    Dictionary<Item, int> resourceCount = new Dictionary<Item, int>();
    // the clues will be keeped

    // enviroment condition
    List<Pickable> pickableItems = new List<Pickable>();
    public GameObject rainTrigger;
    bool rainIsOn = false;
    int enemyCount = 0; // those enemies who found the player

    public int respawnTimes = 0;
    public float startTime; // to count the total amount of time
    public float gameTotalTime = 0;

    public delegate void OnPlayerDie();
    public OnPlayerDie onPlayerDieCallBack;

    public delegate void OnPlayerRespawn();
    public OnPlayerRespawn OnPlayerRespawnCallBack;

    public delegate void OnPlayerLeaveSafeArea();
    public OnPlayerLeaveSafeArea OnPlayerLeaveSafeAreaCallBack;

    public delegate void OnPlayerEnterSafeArea();
    public OnPlayerEnterSafeArea OnPlayerEnterSafeAreaCallBack;

    void Start()
    {
        SoundManager.instance?.PlayBGM("MainBGM");
        startTime = Time.realtimeSinceStartup;
        SaveGame();
    }

    public void PlayerLeaveSafeArea()
    {
        playerIsInSafeAreaOrNot = false;
        OnPlayerLeaveSafeAreaCallBack?.Invoke();
    }

    public void PlayerEnterSafeArea()
    {
        playerIsInSafeAreaOrNot = true;
        OnPlayerEnterSafeAreaCallBack?.Invoke();
    }

    public void FoundPlayer() // enemy
    {
        enemyCount++;
        if (enemyCount == 1)
            SoundManager.instance?.PlayBGM("ChasingBGM");
    }

    public void LosePlayer() // enemy
    {
        enemyCount--;
        if (enemyCount == 0)
            SoundManager.instance?.PlayBGM("MainBGM");
        else if (enemyCount < 0)
            enemyCount = 0;
    }

    public void DisablePlayer()
    {
        Debug.Log("Disable Player");
        disablePlayerCount++;
        if(disablePlayerCount == 1)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerHealth>().enabled = false;
            player.GetComponent<UserControler>().enabled = false;
            gameTotalTime += Time.realtimeSinceStartup - startTime;
        }
    }

    public void EnablePlayer()
    {
        Debug.Log("Enable Player");
        disablePlayerCount--;
        if(disablePlayerCount <= 0)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerHealth>().enabled = true;
            player.GetComponent<UserControler>().enabled = true;
            startTime = Time.realtimeSinceStartup;
        }
        if (disablePlayerCount < 0)
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
            Time.timeScale = 0f;
            DisablePlayer();
        }
    }

    public void RestartTime()
    {
        Debug.Log("Restart Time");
        stopTimeCount--;
        if (stopTimeCount <= 0)
        {
            if(DialogueManager.instance.IsDialogueStarted())
                Time.timeScale = 0.1f;
            else
                Time.timeScale = 1f;
            EnablePlayer();
        }
        if (stopTimeCount < 0)
            stopTimeCount = 0;
    }

    public void SaveGame()
    {
        // player condition
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;

        // inventory condition
        if(resourceCount != null)
            resourceCount.Clear();
        resourceCount = new Dictionary<Item, int>(Inventory.instance.GetResourceCount());  // deep copy

        // enviroment condition
        if(pickableItems != null)
            pickableItems.Clear();
        Pickable[] tempPickableItems = FindObjectsOfType<Pickable>();
        foreach (Pickable item in tempPickableItems)
        {
            if(item.item.itemData.isResource)
                pickableItems.Add(item);
        }
        rainIsOn = rainTrigger.activeSelf;
    }

    public void LoadGame()
    {
        PlayerLeaveSafeArea();

        // player condition
        player.transform.position = new Vector3(playerPosition[0], playerPosition[1], playerPosition[2]);
        player.GetComponent<PlayerHealth>().RecoverHealth();
        player.GetComponent<PlayerMovement>().RecoverEnergy();
        player.GetComponent<FrashlightUser>().TurnOffFrashlight();
        player.GetComponent<FrashlightUser>().RecoverPower();

        // inventory condition
        Inventory.instance.ResetData(resourceCount);

        // enviroment condition
        Pickable[] tempPickableItems = FindObjectsOfType<Pickable>();
        foreach(Pickable item in tempPickableItems)
        {
            if (item.item.itemData.isResource)
                item.gameObject?.SetActive(false);
        }

        foreach (Pickable item in pickableItems)
        {
            item.gameObject?.SetActive(true);
        }

        rainTrigger?.SetActive(false);

        if (rainIsOn)
            rainTrigger?.SetActive(true);
        else
            rainTrigger?.SetActive(false);

        StartCoroutine(RespawnWaiting());
    }

    IEnumerator RespawnWaiting()
    {
        UIManager.instance.CloseLossPanel();
        UIManager.instance.OpenLoadingPanel();

        yield return new WaitForSeconds(8f);

        PlayerRespawn();
    }

    void PlayerRespawn()
    {
        UIManager.instance.CloseLoadingPanel();
        respawnTimes++;
        EnablePlayer();
        isPlayerAlive = true;
        OnPlayerRespawnCallBack?.Invoke();
    }

    public void PlayerDie()
    {
        UIManager.instance?.OpenLossPanel();
        DisablePlayer();
        PlayerEnterSafeArea();
        isPlayerAlive = false;
        onPlayerDieCallBack?.Invoke();
        SoundManager.instance?.PlayBGM("MainBGM");
    }

    public float GetGamingTime()
    {
        gameTotalTime += Time.realtimeSinceStartup - startTime;
        startTime = Time.realtimeSinceStartup;
        return gameTotalTime;
    }
}
