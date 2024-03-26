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
    private List<CombatAgent> _waitingFor = new List<CombatAgent>();

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
    
    public virtual IEnumerator Animate()
    {
        yield return PlayAnimation(Animator, AnimationName);
        _waitingFor.Clear();
        foreach (var target in Targets)
        {
            target.StartCoroutine(PlayHitAnimations(target));
        }
        yield return new WaitWhile(() => _waitingFor.Count < Targets.Count);
    }

    private IEnumerator PlayHitAnimations(CombatAgent target)
    {
        yield return target.AnimateHit();
        _waitingFor.Add(target);
    }

    private IEnumerator PlayAnimation(Animator animator, string animationName)
    {
        if (animator != null && animator.HasState(0, Animator.StringToHash(animationName)))
        {
            animator.Play(animationName);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
