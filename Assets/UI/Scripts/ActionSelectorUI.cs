using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionSelectorState
{
    SelectingAction,
    SelectingTarget,
}

public class ActionSelectorUI : MonoBehaviour
{
    public List<Action> actions;
    [SerializeField] private PlayerAgent playerAgent;
    [SerializeField] private ActionButtonUI actionButtonPrefab;
    [SerializeField] private GridLayoutGroup buttonGrid;
    private CombatManager _combatManager;
    private ActionSelectorState _state = ActionSelectorState.SelectingAction;
    private Action _selectedAction;
    private List<CombatAgent> _selectedTargets;

    private void Start()
    {
        _combatManager = FindObjectOfType<CombatManager>();
        foreach (var action in actions)
        {
            ActionButtonUI actionButton = Instantiate(actionButtonPrefab, buttonGrid.transform).SetAction(action);
            actionButton.GetComponent<Button>().onClick.AddListener(() => OnActionSelected(action));
        }
    }

    private void Update()
    {
        if (_state == ActionSelectorState.SelectingTarget && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            OnTargetsSelected(_selectedTargets);
        }
    }

    private void OnActionSelected(Action action)
    {
        _selectedAction = action;
        _state = ActionSelectorState.SelectingTarget;
        if (action.TargetType == TargetType.Self)
        {
            _selectedTargets = new List<CombatAgent> { };
        }
        else 
        if (action.TargetType == TargetType.SingleTarget)
        {
            _selectedTargets = new List<CombatAgent> { _combatManager.Enemies[0] };
        }
        else if (action.TargetType == TargetType.AOE)
        {
            _selectedTargets = new List<CombatAgent>(_combatManager.Enemies);
        }
    }

    private void OnTargetsSelected(List<CombatAgent> targets)
    {
        playerAgent.QueueAction(_selectedAction.CreateWithUserAndTargets(playerAgent, targets));
        _state = ActionSelectorState.SelectingAction;
        _selectedAction = null;
    }
}
