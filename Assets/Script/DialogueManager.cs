﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text dialogueText;
    bool isTypingSentence = false;

    #region Singleton
    public static DialogueManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one of instance of Dialogue Manager found!");
            return;
        }
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        speakerNameText.text = dialogue.speakerName;
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Peek();

        if(isTypingSentence)
        {
            StopAllCoroutines();
            isTypingSentence = false;
            dialogueText.text = sentence;
            sentences.Dequeue();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTypingSentence = true;
        dialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        sentences.Dequeue();
        isTypingSentence = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        sentences.Clear();
    }

    private void Update()
    {
        if(Input.GetButtonDown("NextSentence"))
        {
            DisplayNextSentence();
        }
    }
}
