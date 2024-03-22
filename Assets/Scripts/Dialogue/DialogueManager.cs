using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Animator _textBoxAnim;
    private DialogueTrigger _currentTrigger;
    private Queue<string> _sentences;


    //The below region just creates a reference of this specific controller that we can call from other scripts quickly
    #region Singleton

    public static DialogueManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of playercontroller present!! NOT GOOD!");
            return;
        }

        instance = this;
    }

    #endregion

    private void Start()
    {
        _sentences = new Queue<string>();
    }

    //Begins a conversation when the dialogue trigger is activated from an NPC or item
    public void StartDialogue(Dialogue _dialogue, DialogueTrigger _trigger)
    {
        _currentTrigger = _trigger;

        if (_textBoxAnim != null)
            _textBoxAnim.SetBool("active", true);

        _sentences.Clear();

        //uses FIFO to queue up each sentence in our public dialogue box
        foreach (string sentence in _dialogue._sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        //Takes the next-added sentence out of the queue and loads it into the text box
        string _loadedText = _sentences.Dequeue();
        _dialogueText.text = _loadedText;
        Debug.Log(_loadedText);
    }

    public void EndDialogue()
    {
        Debug.Log("End of Conversation");
        if (_textBoxAnim != null)
            _textBoxAnim.SetBool("active", false);
        _dialogueText.text = "";
        if (_currentTrigger)
        {
            //This line will cause errors until we actually make the trigger!
            _currentTrigger._startedTalking = false;
        }
    }

    public void CallTimerEnd(float _textTime)
    {
        StartCoroutine(TimerEnd(_textTime));
    }

    private IEnumerator TimerEnd(float _textTime)
    {
        yield return new WaitForSeconds(_textTime);
        EndDialogue();
    }
}
