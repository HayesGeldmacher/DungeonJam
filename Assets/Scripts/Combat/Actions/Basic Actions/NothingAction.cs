using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Actions/Nothing")]
public class NothingAction : Action
{
    public override void Execute()
    {
        Debug.Log("Nothing happens");
    }
}
