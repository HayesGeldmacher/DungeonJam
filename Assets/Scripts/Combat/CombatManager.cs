using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] public float TurnDuration { get; private set; } = 5.0f;
    public float TurnTime { get; private set; } = 0.0f;
    public float NormalizedTurnTime => TurnTime / TurnDuration;

    public List<CombatAgent> Agents = new List<CombatAgent>();

    private void Awake()
    {
        Agents.AddRange(FindObjectsOfType<CombatAgent>());
    }

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
        foreach (var agent in Agents)
        {
            agent.GetNextAction()?.Execute();
        }
    }
}
