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
    [SerializeField] private PlayerAgent _playerAgent;
    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private TargetReticleUI _targetReticlePrefab;
    [SerializeField] private GridLayoutGroup _buttonGrid;
    [SerializeField] private CombatBarUI _actionBar;
    private CombatManager _combatManager;
    private ActionSelectorState _state = ActionSelectorState.SelectingAction;
    private Action _selectedAction;
    private Animator _animator;
    private List<CombatAgent> _selectedTargets = new List<CombatAgent>();
    private Dictionary<CombatAgent, TargetReticleUI> _targetReticles = new Dictionary<CombatAgent, TargetReticleUI>();
    private Dictionary<KeyCode, Action> _actionKeys = new Dictionary<KeyCode, Action>();

    private void Start()
    {
        _combatManager = FindObjectOfType<CombatManager>();
        foreach (var action in _playerAgent.LeftHandActions)
        {
            ActionButtonUI actionButton = Instantiate(_actionButtonPrefab, _buttonGrid.transform).SetAction(action);
            actionButton.GetComponent<Button>().onClick.AddListener(() => {
                    _animator = _playerAgent.LeftHandAnimator;
                    OnActionSelected(action);
            });
        }
        foreach (var action in _playerAgent.GenericActions)
        {
            ActionButtonUI actionButton = Instantiate(_actionButtonPrefab, _buttonGrid.transform).SetAction(action);
            actionButton.GetComponent<Button>().onClick.AddListener(() => OnActionSelected(action));
        }
        foreach (var action in _playerAgent.RightHandActions)
        {
            ActionButtonUI actionButton = Instantiate(_actionButtonPrefab, _buttonGrid.transform).SetAction(action);
            actionButton.GetComponent<Button>().onClick.AddListener(() => {
                    _animator = _playerAgent.RightHandAnimator;
                    OnActionSelected(action);
            });
        }
        _actionKeys[KeyCode.Q] = _playerAgent.LeftHandActions[0];
        _actionKeys[KeyCode.A] = _playerAgent.LeftHandActions[1];
        _actionKeys[KeyCode.W] = _playerAgent.GenericActions[0];
        _actionKeys[KeyCode.S] = _playerAgent.GenericActions[1];
        _actionKeys[KeyCode.E] = _playerAgent.RightHandActions[0];
        _actionKeys[KeyCode.D] = _playerAgent.RightHandActions[1];
        foreach (var agent in _combatManager.Agents)
        {
            TargetReticleUI targetReticle = Instantiate(_targetReticlePrefab, transform);
            targetReticle.SetAgent(agent);
            _targetReticles[agent] = targetReticle;
        }
    }

    private void Update()
    {

        if (_state == ActionSelectorState.SelectingAction)
        {
            foreach (var kvp in _actionKeys)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    OnActionSelected(kvp.Value);
                    if (kvp.Key == KeyCode.Q || kvp.Key == KeyCode.A)
                    {
                        _animator = _playerAgent.LeftHandAnimator;
                    }
                    else if (kvp.Key == KeyCode.E || kvp.Key == KeyCode.D)
                    {
                        _animator = _playerAgent.RightHandAnimator;
                    }
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
                    if (index >= _combatManager.Enemies.Count) index = _combatManager.Enemies.Count - 1;
                    _selectedTargets = new List<CombatAgent> { _combatManager.Enemies[index] };
                    UpdateTargetIcons();
                }
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
                {
                    int index = _combatManager.Enemies.IndexOf(_selectedTargets[0]) - 1;
                    if (index < 0) index = 0;
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
        _playerAgent.QueueAction(_selectedAction.CreateWithUserAndTargets(_playerAgent, targets).SetAnimator(_animator));
        _state = ActionSelectorState.SelectingAction;
        _selectedAction = null;
        _animator = null;
        _selectedTargets = new List<CombatAgent>();
        UpdateTargetIcons();
    }

    private void UpdateTargetIcons()
    {
        foreach (var agent in _combatManager.Agents)
        {
            var bar = _actionBar.AgentInfoBars[agent];
            var reticle = _targetReticles[agent];
            bar.SetTargeted(_selectedTargets.Contains(agent));
            reticle.SetTargeted(_selectedTargets.Contains(agent));
        }
        if (_selectedAction != null && _selectedAction.TargetType == TargetType.Self)
        {
            _actionBar.AgentInfoBars[_playerAgent].SetTargeted(true);
        }
    }
}
