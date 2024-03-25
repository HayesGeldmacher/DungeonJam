using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : CombatAgent
{
    [SerializeField] private Action _primaryAction;
    [SerializeField] private Action _secondaryAction;

    public List<Action> GenericActions = new List<Action>();
    public Animator LeftHandAnimator;
    public List<Action> LeftHandActions = new List<Action>();
    public Animator RightHandAnimator;
    public List<Action> RightHandActions = new List<Action>();

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         QueueAction(_primaryAction);
    //     }
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         QueueAction(_secondaryAction);
    //     }
    //
}
