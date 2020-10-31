using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue = null;

    public void TriggerDialogue()
    {
        if(dialogue != null)
            DialogueManager.instance?.StartDialogue(dialogue);
    }
}
