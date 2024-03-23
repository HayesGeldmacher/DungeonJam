using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAgent : MonoBehaviour
{
    private Queue<Action> _actions = new Queue<Action>();
    private Stack<Action> _actionHistory = new Stack<Action>();

    [SerializeField] private Action _preparationAction;
    [SerializeField] private Action _recoveryAction;
    [SerializeField] private Action _nothingAction;
    
    public void QueueAction(Action action)
    {
        for (int i = 0; i < action.PreparationTurns; i++)
        {
            _actions.Enqueue(Instantiate(_preparationAction));
        }
        _actions.Enqueue(action);
        for (int i = 0; i < action.RecoveryTurns; i++)
        {
            _actions.Enqueue(Instantiate(_recoveryAction));
        }
    }

    public Action GetNextAction()
    {
        Action action = _actions.Count > 0 ? _actions.Dequeue() : Instantiate(_nothingAction);
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
