using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Recovery")]
public class RecoveryAction : Action
{
    public override void Execute(CombatManager context)
    {
        Debug.Log("Recovery Action Executed");
    }

}
