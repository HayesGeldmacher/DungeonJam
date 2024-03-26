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
    public string AnimationName;
    public int PreparationTurns;
    public int RecoveryTurns;
    public abstract TargetType TargetType { get; protected set; }
    public abstract bool Priority { get; protected set; }

    public CombatAgent User { get; private set; } = null;
    public List<CombatAgent> Targets { get; private set; } = new List<CombatAgent>();
    public Animator Animator { get; private set; } = null;

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

    public Action SetAnimator(Animator animator)
    {
        Animator = animator;
        return this;
    }

    public virtual string GetDescription() => "";

    public abstract void Execute(CombatManager context);
    
    public void Animate()
    {
        if (Animator != null && AnimationName != null && Animator.HasState(0, Animator.StringToHash(AnimationName)))
        {
            Animator.Play(AnimationName);
        }
    }
}
