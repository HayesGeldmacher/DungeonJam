using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int PreparationTurns;
    public int RecoveryTurns;
    [HideInInspector] public CombatAgent User { get; private set; } = null;
    [HideInInspector] public List<CombatAgent> Targets { get; private set; } = new List<CombatAgent>();

    public Action SetUser(CombatAgent user)
    {
        User = user;
        return this;
    }

    public Action SetTargets(List<CombatAgent> targets)
    {
        Targets = targets;
        return this;
    }

    public abstract void Execute(CombatManager context);
}