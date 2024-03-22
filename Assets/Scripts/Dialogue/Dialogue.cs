using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialogue
{
    //This script is never actually placed on an object
    //The Interactable script uses it as a way to add dialogue to objects

    [TextArea(3, 10)]
    public string[] _sentences;
}
