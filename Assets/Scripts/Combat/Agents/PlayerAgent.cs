using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : CombatAgent
{
    [SerializeField] private Action _primaryAction;
    [SerializeField] private Action _secondaryAction;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            QueueAction(Instantiate(_primaryAction));
        }
        if (Input.GetMouseButtonDown(1))
        {
            QueueAction(Instantiate(_secondaryAction));
        }
    }
}
