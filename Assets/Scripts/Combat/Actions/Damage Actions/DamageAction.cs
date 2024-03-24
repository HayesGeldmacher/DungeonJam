using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageAction : Action
{
    public override bool Priority { get; protected set; } = false;
    [SerializeField] private int _damage;

    public override void Execute(CombatManager context)
    {
        foreach (var target in Targets)
        {
            target.TakeDamage(_damage);
            Debug.Log($"{target.name} took {_damage} damage.");
        }
    }
}
