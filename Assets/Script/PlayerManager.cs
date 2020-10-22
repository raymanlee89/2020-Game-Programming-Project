using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

	public static PlayerManager instance;

	void Awake(){

        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of Inventory found!");
            return;
        }
        instance = this;
	}

    #endregion

    public GameObject player;
}
