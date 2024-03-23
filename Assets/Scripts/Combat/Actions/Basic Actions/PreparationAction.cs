using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Preparation")]
public class PreparationAction : Action
{
    public override void Execute()
    {
        Debug.Log("Preparation Action Executed");
    }
}
