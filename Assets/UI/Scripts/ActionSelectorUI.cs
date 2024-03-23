using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelectorUI : MonoBehaviour
{
    public List<Action> actions;
    [SerializeField] private PlayerAgent playerAgent;
    [SerializeField] private ActionButtonUI actionButtonPrefab;
    [SerializeField] private GridLayoutGroup buttonGrid;

    private void Start()
    {
        foreach (var action in actions)
        {
            ActionButtonUI actionButton = Instantiate(actionButtonPrefab, buttonGrid.transform).SetAction(action);
            actionButton.GetComponent<Button>().onClick.AddListener(() => playerAgent.QueueAction(action));
        }
    }
}
