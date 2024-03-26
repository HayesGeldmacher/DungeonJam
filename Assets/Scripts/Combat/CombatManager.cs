using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] public float TurnDuration { get; private set; } = 5.0f;
    [field: SerializeField] public float DelayBetweenActions { get; private set; } = 0.2f;
    public float TurnTime { get; private set; } = 0.0f;
    public float NormalizedTurnTime => TurnTime / TurnDuration;

    public delegate void ActionHandler(Action action);
    public event ActionHandler OnActionStart;
    public event ActionHandler OnActionEnd;

    public CombatAgent Player;
    public List<CombatAgent> Enemies = new List<CombatAgent>();
    public List<CombatAgent> Agents { get => new List<CombatAgent> { Player }.Concat(Enemies).ToList(); }
    private bool _processingTurn = false;

    private void Update()
    {
        if (_processingTurn) return;
        
        TurnTime += Time.deltaTime;
        if (TurnTime >= TurnDuration)
        {
            TurnTime = 0.0f;
            StartCoroutine(ProcessTurn());
        }
    }

    private bool IsRealAction(Action action)
    {
        return action != null && !(action is NothingAction) && !(action is PreparationAction) && !(action is RecoveryAction);
    }

    private IEnumerator ProcessTurn()
    {
        _processingTurn = true;
        Dictionary<CombatAgent, Action> actions = new Dictionary<CombatAgent, Action>();
        foreach (var agent in Agents)
        {
            actions.Add(agent, agent.GetNextAction());
        }
        List<CombatAgent> orderedAgents = new List<CombatAgent>() { Player }.Concat(Enemies).ToList();
        // TODO: Buffs / Effects should be processed here

        foreach (var agent in orderedAgents.Where(agent => actions[agent].Priority))
        {
            yield return ProcessAction(actions[agent]);
        }
        foreach (var agent in orderedAgents.Where(agent => !actions[agent].Priority))
        {
            yield return ProcessAction(actions[agent]);
        }
        _processingTurn = false;
    }

    private IEnumerator ProcessAction(Action action)
    {
        OnActionStart?.Invoke(action);
        if (IsRealAction(action))
        {
            action.Animate();
            yield return new WaitForEndOfFrame();
            float animationTime = action.Animator == null ? 0 : action.Animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationTime + DelayBetweenActions);
        }
        action.Execute(this);
        OnActionEnd?.Invoke(action);
    }
}
