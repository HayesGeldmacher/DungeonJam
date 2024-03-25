using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatTrigger : Interactable
{
    public override void OnInteract()
    {
        SceneManager.LoadScene("Combat Scene");
    }
}
