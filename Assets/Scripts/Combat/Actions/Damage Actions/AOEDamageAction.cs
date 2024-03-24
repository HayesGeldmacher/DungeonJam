using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageAction", menuName = "Actions/Damage/AOE Damage")]
public class AOEDamageAction : DamageAction
{
    public override TargetType TargetType { get; protected set; } = TargetType.AOE;
}
