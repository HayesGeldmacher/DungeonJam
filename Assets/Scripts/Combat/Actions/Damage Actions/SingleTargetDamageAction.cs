using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageAction", menuName = "Actions/Damage/Single Target Damage")]
public class SingleTargetDamageAction : DamageAction
{
    public override TargetType TargetType { get; protected set; } = TargetType.SingleTarget;
}
