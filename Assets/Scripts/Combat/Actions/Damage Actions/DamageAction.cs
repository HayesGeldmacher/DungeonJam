using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageAction : Action
{
    public override bool Priority { get; protected set; } = false;
    [SerializeField] private int _damage;

    public override string GetDescription()
    {
        return $"{Targets[0].name} took {_damage} damage.";
    }

    public override void Execute(CombatManager context)
    {
        foreach (var target in Targets)
        {
            target.TakeDamage(_damage);
        }
    }
}
