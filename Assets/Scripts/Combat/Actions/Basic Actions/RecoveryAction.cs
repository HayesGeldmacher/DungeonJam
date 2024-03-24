using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Basic/Recovery")]
public class RecoveryAction : Action
{
    public override bool Priority { get; protected set; } = false;
    public override TargetType TargetType { get; protected set; } = TargetType.Self;

    public override void Execute(CombatManager context)
    {
        Debug.Log("Recovery Action Executed");
    }

}
