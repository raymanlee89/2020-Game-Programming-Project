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

    // player condition
    float[] playerPosition = new float[3];

    // inventory condition
    Dictionary<ItemData, int> resourceCount;
    List<ItemData> clues;

    // enviroment condition
    Pickable[] pickableItems;
    Openable[] doors;
    bool[] isDoorOpened = null;
    public GameObject rainTrigger;
    bool rainIsOn = false;

    public void DisablePlayer()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<UserControler>().enabled = false;
    }

    public void EnablePlayer()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<UserControler>().enabled = true;
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
