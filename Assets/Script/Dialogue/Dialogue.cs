using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public Speaker speaker = Speaker.Player;

    public enum Speaker
    {
        Player,
        Friend,
        Unknown
    }

    [TextArea(1, 10)]
    public string[] sentences;
    public int[] faceIndexes;
}
