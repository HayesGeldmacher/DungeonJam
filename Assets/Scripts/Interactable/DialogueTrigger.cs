using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable
{
    //This script is a parent script that many objects will inherit from


    [Header("Dialogue Variables")]
    [SerializeField] protected bool _canTalk = false;
    [HideInInspector] public bool _startedTalking = false;
    [SerializeField] protected bool _canWalkAway;
    [SerializeField] private float _dialogueTimer = 1;
    [SerializeField] protected float _dialogueDistance = 5;
    private DialogueManager _manager;
    public Dialogue _dialogue;

    [Header("Item Variables")]
    [SerializeField] private bool _canBeGrabbed;

    public virtual void Start()
    {
        _manager = DialogueManager.instance;
    }

    public override void OnInteract()
    {
        if (_canTalk)
        {
            TriggerDialogue();
        }
    }

    public virtual void TriggerDialogue()
    {
        if (!_startedTalking)
        {
            DialogueTrigger _trigger = transform.GetComponent<DialogueTrigger>();
            _manager.StartDialogue(_dialogue, this);
            _startedTalking = true;
        }
        else
        {
            _manager.DisplayNextSentence();
        }
    }

    public virtual void EndDialogue()
    {
        _manager.EndDialogue();
    }

    public virtual void CallEndDialogue()
    {
        _manager.CallTimerEnd(_dialogueTimer);
    }

    public virtual void PickUpItem()
    {
        //have all the pickup stuff happen here
        if (_canTalk)
        {
            _manager.CallTimerEnd(_dialogueTimer);
        }
        Destroy(gameObject);

    }
}