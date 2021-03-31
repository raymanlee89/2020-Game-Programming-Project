using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData{
    // player condition
    public float[] playerPosition;

    // inventory condition
    public Dictionary<Item, int> resourceCount;
    public List<Item> clues;

    public GameData (Transform playerTransform, Inventory inventory)
    {
        playerPosition = new float[3];
        playerPosition[0] = playerTransform.position.x;
        playerPosition[1] = playerTransform.position.y;
        playerPosition[2] = playerTransform.position.z;

        resourceCount = inventory.GetResourceCount();
        clues = inventory.GetCluesList();
    }
}
