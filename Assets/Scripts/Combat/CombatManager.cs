using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] public float TurnDuration { get; private set; } = 5.0f;
    [field: SerializeField] public float ActionTime { get; private set; } = 0.2f;
    public float TurnTime { get; private set; } = 0.0f;
    public float NormalizedTurnTime => TurnTime / TurnDuration;

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

    private IEnumerator ProcessTurn()
    {
        _processingTurn = true;
        Dictionary<CombatAgent, Action> actions = new Dictionary<CombatAgent, Action>();
        foreach (var agent in Agents)
        {
            actions.Add(agent, agent.GetNextAction());
        }
        // TODO: Buffs / Effects should be processed here
        if (actions[Player].Priority)
        {
            actions[Player].Animate();
            yield return new WaitForSeconds(ActionTime);
            actions[Player].Execute(this);
        }
        foreach (var enemy in Enemies)
        {
            if (actions[enemy].Priority)
            {
                actions[enemy].Animate();
                yield return new WaitForSeconds(ActionTime);
                actions[enemy].Execute(this);
            }
        }
        if (!actions[Player].Priority)
        {
            actions[Player].Animate();
            yield return new WaitForSeconds(ActionTime);
            actions[Player].Execute(this);
        }
        foreach (var enemy in Enemies)
        {
            if (!actions[enemy].Priority)
            {
                actions[enemy].Animate();
                yield return new WaitForSeconds(ActionTime);
                actions[enemy].Execute(this);
            }
        }
        _processingTurn = false;
    }
}
