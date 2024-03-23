using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int PreparationTurns;
    public int RecoveryTurns;

    public abstract void Execute();
}
