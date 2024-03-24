using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    Self,
    SingleTarget,
    AOE,
}

public abstract class Action : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int PreparationTurns;
    public int RecoveryTurns;
    public abstract TargetType TargetType { get; protected set; }
    public abstract bool Priority { get; protected set; }

    public CombatAgent User { get; private set; } = null;
    public List<CombatAgent> Targets { get; private set; } = new List<CombatAgent>();

    public Action CreateWithUser(CombatAgent user)
    {
        return CreateWithUserAndTargets(user, new List<CombatAgent> {});
    }

    public Action CreateWithUserAndTarget(CombatAgent user, CombatAgent target)
    {
        return CreateWithUserAndTargets(user, new List<CombatAgent> { target });
    }

    public Action CreateWithUserAndTargets(CombatAgent user, List<CombatAgent> targets)
    {
        return Instantiate(this).SetUser(user).SetTargets(targets);
    }

    private Action SetUser(CombatAgent user)
    {
        User = user;
        return this;
    }

    private Action SetTargets(List<CombatAgent> targets)
    {
        Targets = targets;
        return this;
    }

    public abstract void Execute(CombatManager context);
}
