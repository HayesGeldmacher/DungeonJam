using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string PromptText = "Press ? to interact";

    public virtual void OnFocus() { }
    public virtual void OnUnfocus() { }
    public abstract void OnInteract();
}
