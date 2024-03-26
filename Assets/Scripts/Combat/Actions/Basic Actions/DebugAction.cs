using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugAction", menuName = "Actions/Basic/Debug")]
public class DebugAction : Action
{
    public override bool Priority { get; protected set; } = false;
    public override TargetType TargetType { get; protected set; } = TargetType.Self;

    public override void Execute(CombatManager context)
    {
        // Debug.Log("Debug Action Executed");
    }
}
