using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] public float TurnDuration { get; private set; } = 5.0f;
    public float TurnTime { get; private set; } = 0.0f;
    public float NormalizedTurnTime => TurnTime / TurnDuration;

    public CombatAgent Player;
    public List<CombatAgent> Enemies = new List<CombatAgent>();
    public List<CombatAgent> Agents { get => new List<CombatAgent> { Player }.Concat(Enemies).ToList(); }

    private void Update()
    {
        TurnTime += Time.deltaTime;
        if (TurnTime >= TurnDuration)
        {
            TurnTime = 0.0f;
            ProcessTick();
        }
    }

    private void ProcessTick()
    {
        List<Action> actions = new List<Action>();
        foreach (var agent in Agents)
        {
            actions.Add(agent.GetNextAction());
        }
        foreach (var action in actions)
        {
            if (action.Priority)
            {
                action.Execute(this);
            }
        }
        foreach (var action in actions)
        {
            if (!action.Priority)
            {
                action.Execute(this);
            }
        }
    }
}
