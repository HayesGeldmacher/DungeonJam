using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageAction", menuName = "Actions/DamageAction")]
public class DamageAction : Action
{
    [Header("Damage Action")]
    [SerializeField] private int damage;

    public override void Execute(CombatManager context)
    {
        foreach (var agent in Targets)
        {
            agent.TakeDamage(damage);
            Debug.Log($"{User.name} dealt {damage} damage to {agent.name}.");
        }
    }
}
