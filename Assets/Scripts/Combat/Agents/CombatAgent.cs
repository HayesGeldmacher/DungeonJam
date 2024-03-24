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

    private Queue<Action> _actions = new Queue<Action>();
    private Stack<Action> _actionHistory = new Stack<Action>();

    [SerializeField] protected Action _preparationAction;
    [SerializeField] protected Action _recoveryAction;
    [SerializeField] protected Action _nothingAction;
    
    protected virtual void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= Mathf.Abs(damage - DamageNegation);
    }

    public void CreateAndQueueAction(Action action, CombatAgent target)
    {
        CreateAndQueueAction(action, new List<CombatAgent> { target });
    }
    
    public void CreateAndQueueAction(Action action, List<CombatAgent> targets)
    {
        action = Instantiate(action).SetUser(this).SetTargets(targets);
        // Maybe do other things with the action here
         
        for (int i = 0; i < action.PreparationTurns; i++)
        {
            _actions.Enqueue(Instantiate(_preparationAction).SetUser(this));
        }
        _actions.Enqueue(action);
        for (int i = 0; i < action.RecoveryTurns; i++)
        {
            _actions.Enqueue(Instantiate(_recoveryAction).SetUser(this));
        }
    }

    public Action GetNextAction()
    {
        Action action = _actions.Count > 0 ? _actions.Dequeue() : Instantiate(_nothingAction).SetUser(this);
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
}
