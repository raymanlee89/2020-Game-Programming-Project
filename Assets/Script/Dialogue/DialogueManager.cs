using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text dialogueText;
    public string playerName;
    public string friendName;
    bool isTypingSentence = false;

    public delegate void OnDialogueEnd();
    public OnDialogueEnd onDialogueEndCallBack;

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
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0.1f;
        GameManager.instance.DisablePlayer();
        dialoguePanel.SetActive(true);
        if (dialogue.speaker == Dialogue.Speaker.Player)
            speakerNameText.text = playerName;
        else if (dialogue.speaker == Dialogue.Speaker.Friend)
            speakerNameText.text = friendName;
        else
            speakerNameText.text = "???";
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
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

        //StartCoroutine(AutoNextCountdown());
        Debug.Log("Start auto next countdown");
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTypingSentence = true;
        dialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        sentences.Dequeue();
        isTypingSentence = false;
    }

    IEnumerator AutoNextCountdown()
    {
        yield return new WaitForSeconds(5f);
        if(sentences.Count > 0)
            DisplayNextSentence();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        sentences.Clear();
        GameManager.instance.EnablePlayer();
        onDialogueEndCallBack?.Invoke();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Input.GetButtonDown("NextSentence") && Time.timeScale != 0 && dialoguePanel.activeSelf)
        {
            DisplayNextSentence();
        }
    }
}
