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
    [SerializeField] private PlayerAgent _playerAgent;
    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private GridLayoutGroup _buttonGrid;
    [SerializeField] private CombatBarUI _actionBar;
    private CombatManager _combatManager;
    private ActionSelectorState _state = ActionSelectorState.SelectingAction;
    private Action _selectedAction;
    private List<CombatAgent> _selectedTargets = new List<CombatAgent>();
    private Dictionary<KeyCode, int> _actionIndex = new Dictionary<KeyCode, int>
    {
        { KeyCode.Q, 0 },
        { KeyCode.A, 1 },
        { KeyCode.W, 2 },
        { KeyCode.S, 3 },
        { KeyCode.E, 4 },
        { KeyCode.D, 5 },
    };

    private void Start()
    {
        _combatManager = FindObjectOfType<CombatManager>();
        foreach (var action in actions)
        {
            ActionButtonUI actionButton = Instantiate(_actionButtonPrefab, _buttonGrid.transform).SetAction(action);
            actionButton.GetComponent<Button>().onClick.AddListener(() => OnActionSelected(action));
        }
    }

    private void Update()
    {
        if (_state == ActionSelectorState.SelectingAction)
        {
            foreach (var kvp in _actionIndex)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    OnActionSelected(actions[kvp.Value]);
                }
            }
        }
        else if (_state == ActionSelectorState.SelectingTarget)
        {
            if (_selectedAction.TargetType == TargetType.SingleTarget)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    int index = _combatManager.Enemies.IndexOf(_selectedTargets[0]) + 1;
                    if (index >= _combatManager.Enemies.Count) index = 0;
                    _selectedTargets = new List<CombatAgent> { _combatManager.Enemies[index] };
                    UpdateTargetIcons();
                }
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
                {
                    int index = _combatManager.Enemies.IndexOf(_selectedTargets[0]) - 1;
                    if (index < 0) index = _combatManager.Enemies.Count - 1;
                    _selectedTargets = new List<CombatAgent> { _combatManager.Enemies[index] };
                    UpdateTargetIcons();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnTargetsSelected(_selectedTargets);
            }
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
        UpdateTargetIcons();
    }

    private void OnTargetsSelected(List<CombatAgent> targets)
    {
        _playerAgent.QueueAction(_selectedAction.CreateWithUserAndTargets(_playerAgent, targets));
        _state = ActionSelectorState.SelectingAction;
        _selectedAction = null;
        _selectedTargets = new List<CombatAgent>();
        for (int i = 0; i < _combatManager.Enemies.Count; i++)
        {
            var bar = _actionBar.AgentInfoBars[_combatManager.Enemies[i]];
            bar.SetTargeted(false);
        }
    }

    private void UpdateTargetIcons()
    {
        foreach (var agent in _combatManager.Agents)
        {
            var bar = _actionBar.AgentInfoBars[agent];
            bar.SetTargeted(_selectedTargets.Contains(agent));
        }
        if (_selectedAction != null && _selectedAction.TargetType == TargetType.Self)
        {
            _actionBar.AgentInfoBars[_playerAgent].SetTargeted(true);
        }
    }
}
