using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAgent : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0;

    public int DamageNegation { get; private set; } = 0;
    public int DamageAmplification { get; private set; } = 0;

    protected Queue<Action> _actions = new Queue<Action>();
    protected Stack<Action> _actionHistory = new Stack<Action>();

    public Transform TargetedReticlePosition;

    [SerializeField] protected Action _preparationAction;
    [SerializeField] protected Action _recoveryAction;
    [SerializeField] protected Action _nothingAction;
    protected CombatManager _combatManager;
    
    protected virtual void Awake()
    {
        CurrentHealth = MaxHealth;
        _combatManager = FindObjectOfType<CombatManager>();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= Mathf.Abs(damage - DamageNegation);
    }

    public void QueueAction(Action action)
    {
        for (int i = 0; i < action.PreparationTurns; i++)
        {
            _actions.Enqueue(_preparationAction.CreateWithUser(this));
        }
        _actions.Enqueue(action);
        for (int i = 0; i < action.RecoveryTurns; i++)
        {
            _actions.Enqueue(_recoveryAction.CreateWithUser(this));
        }
    }

    public Action GetNextAction()
    {
        Action action = _actions.Count > 0 ? _actions.Dequeue() : _nothingAction.CreateWithUser(this);
        _actionHistory.Push(action);
        return action;
    }

    public List<Action> GetActions()
    {
        return new List<Action>(_actions);
    }

    public List<Action> GetActionHistory()
    {
        return new List<Action>(_actionHistory);
    }

    public abstract IEnumerator AnimateHit();
    public abstract IEnumerator AnimateDeath();
}
