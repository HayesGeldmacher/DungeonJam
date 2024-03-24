using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAgent : CombatAgent
{
    [SerializeField] private Action _action;
    private CombatManager _combatManager;

    protected override void Awake()
    {
        //call the base class's Awake method
        base.Awake();
        _combatManager = FindObjectOfType<CombatManager>();
    }

    private void Update()
    {
        if (GetActions().Count < 5)
        {
            Action action = Random.Range(0f, 1f) < 0.5 ? _action : _nothingAction;
            CreateAndQueueAction(action, new List<CombatAgent> { _combatManager.Player });
        }
    }
}
