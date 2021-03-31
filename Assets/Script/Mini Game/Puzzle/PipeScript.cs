using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    int[] rotations = { 0,90,180,270 }; //define four possible rotations
    public bool real; //distinguish dummy blocks (not within solution)
    public int set_order; //order of real blocks
    public int correctRotation; //mutiple correct directions
    bool isPlaced = false; //if block in correct rotation
    
    PuzzleManager puzzleManager;

    //if the blocks are connected from start (for playing sound)
    private bool connected(bool[] connection, int order)
    {
        bool is_connect = true;
        for (int i = 0; i <= order; i++)
        {
            if(!connection[i])
            {
                is_connect = false;
            }
        }
        return is_connect;
    }

    private void Start()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();

        //assign random rotation to blocks
        int rand = Random.Range(0, rotations.Length);
        this.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, rotations[rand]);
        int Z = (int)this.GetComponent<RectTransform>().eulerAngles.z;

        if(real) //update status after random rotation
        {
            if (Z == correctRotation)
            {
                isPlaced = true;
                puzzleManager.correctMove();
                puzzleManager.flow[set_order] = true;
            }
        }
    }

    public void OnMouseDown()
    {
        if (!puzzleManager.complete)
        {
            SoundManager.instance?.Play("TurnningPipe");
            //rotate 90 degrees when clicked
            this.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 90));
            int Z = (int)this.GetComponent<RectTransform>().eulerAngles.z;
            if(real) //only update data from real blocks
            {
                if (Z == correctRotation && isPlaced == false)
                {
                    isPlaced = true;
                    puzzleManager.correctMove();
                    puzzleManager.flow[set_order] = true;
                    if (connected(puzzleManager.flow, set_order))
                    {
                        //GetComponent<AudioSource>().Play(); //play connected sound
                    }
                }
                else if (isPlaced == true)
                {
                    isPlaced = false;
                    puzzleManager.wrongMove();
                    puzzleManager.flow[set_order] = false;
                }
                else
                { 
                    //Debug.Log("wrongMove1"); 
                }
            }
        }
    }
}
