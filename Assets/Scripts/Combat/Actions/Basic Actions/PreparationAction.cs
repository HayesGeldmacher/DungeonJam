using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Preparation")]
public class PreparationAction : Action
{
    public override void Execute(CombatManager context)
    {
        Debug.Log("Preparation Action Executed");
    }
}
