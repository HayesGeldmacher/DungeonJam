using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugAction", menuName = "Actions/DebugAction")]
public class DebugAction : Action
{
    public override void Execute(CombatManager context)
    {
        Debug.Log("Debug Action Executed");
    }
}
