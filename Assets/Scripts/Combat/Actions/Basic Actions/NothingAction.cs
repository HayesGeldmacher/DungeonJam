using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Actions/Basic/Nothing")]
public class NothingAction : Action
{
    public override bool Priority { get; protected set; } = false;
    public override TargetType TargetType { get; protected set; } = TargetType.Self;

    public override void Execute(CombatManager context)
    {
        // Debug.Log("Nothing happens");
    }
}
