using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageAction", menuName = "Actions/DamageAction")]
public class DamageAction : Action
{
    [Header("Damage Action")]
    [SerializeField] private int damage;

    public override void Execute()
    {
        Debug.Log("Dealing " + damage + " damage!");
    }
}
