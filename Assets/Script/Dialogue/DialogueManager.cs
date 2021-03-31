using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();
    private Queue<int> faceIndexes = new Queue<int>();
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text dialogueText;
    public Image face;
    public List<Sprite> playerFaces;
    public string playerName;
    public string friendName;
    bool isTypingSentence = false;

    public delegate void OnDialogueEnd();
    public OnDialogueEnd onDialogueEndCallBack;
    IEnumerator coroutine = null;

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
        GameManager.instance.onPlayerDieCallBack += EndDialogue;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if(!dialoguePanel.activeSelf)
        {
            Time.timeScale = 0.1f;
            GameManager.instance.DisablePlayer();
            dialoguePanel.SetActive(true);
        }
        if (dialogue.speaker == Dialogue.Speaker.Player)
            speakerNameText.text = playerName;
        else if (dialogue.speaker == Dialogue.Speaker.Friend)
            speakerNameText.text = friendName;
        else
            speakerNameText.text = "???";
        sentences.Clear();
        faceIndexes.Clear();
        for (int i = 0 ; i < dialogue.sentences.Length ; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
            faceIndexes.Enqueue(dialogue.faceIndexes[i]);
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
        int faceIndex = faceIndexes.Peek();
        if (speakerNameText.text == playerName)
            face.sprite = playerFaces[faceIndex];

        if (isTypingSentence)
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                sentences.Dequeue();
                faceIndexes.Dequeue();
            }
            isTypingSentence = false;
            dialogueText.text = sentence;
            
        }
        else
        {
            coroutine = TypeSentence(sentence);
            StartCoroutine(coroutine);
        }

        //StartCoroutine(AutoNextCountdown());
        //Debug.Log("Start auto next countdown");
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTypingSentence = true;
        dialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.008f);
        }
        isTypingSentence = false;
        sentences.Dequeue();
        faceIndexes.Dequeue();
    }

    void EndDialogue()
    {
        if(dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);
            sentences.Clear();
            faceIndexes.Clear();
            GameManager.instance.EnablePlayer();
            onDialogueEndCallBack?.Invoke();
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("NextSentence") && dialoguePanel.activeSelf)
        {
            DisplayNextSentence();
        }
    }

    public bool IsDialogueStarted()
    {
        return dialoguePanel.activeSelf;
    }
}
