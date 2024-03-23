using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAgent : CombatAgent
{
    [SerializeField] private Action _action;

    private void Update()
    {
        if (GetActions().Count < 5)
        {
            QueueAction(Instantiate(Random.Range(0f, 1f) < 0.5 ? _action : new NothingAction()));
        }
    }
}
