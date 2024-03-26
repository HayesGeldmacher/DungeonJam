using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AgentInfoBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform _targetedIcon;
    [SerializeField] private HealthBarUI _healthBar;
    [SerializeField] private RectTransform _line;
    [SerializeField] private ActionIconUI _actionIconPrefab;
    private CombatAgent _combatAgent;
    private CombatBarUI _actionBar;
    private List<ActionIconUI> _actionIcons = new List<ActionIconUI>();

    public void SetCombatAgent(CombatAgent agent)
    {
        _combatAgent = agent;
        _healthBar.SetCombatAgent(agent);
    }

    public void SetTargeted(bool targeted)
    {
        _targetedIcon.gameObject.SetActive(targeted);
    }

    private void Awake()
    {
        _actionBar = GetComponentInParent<CombatBarUI>();
        for (int i = 0; i < _actionBar.TurnCount*2; i++)
        {
            _actionIcons.Add(Instantiate(_actionIconPrefab, _line).SetTurnWidth(_line.rect.width / _actionBar.TurnCount));
        }
        _healthBar.SetCombatAgent(_combatAgent);
    }

    private void Update()
    {
        float x = _actionBar.CombatManager.NormalizedTurnTime;
        x = (x < .1) ? 0 : (x > 0.90) ? 1 : (x - 0.1f) / 0.8f; // ease in out quad
        float normalizedTime = (x < 0.5) ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2; // ease in out cubic

        List<Action> actions = _combatAgent.GetActions();
        List<Action> history = _combatAgent.GetActionHistory();
        while (actions.Count < _actionIcons.Count)
        {
            actions.Add(null);
        }
        while (history.Count < _actionIcons.Count)
        {
            history.Add(null);
        }
        actions = actions.Take(_actionBar.TurnCount).ToList();
        history = history.Take(_actionBar.TurnCount).ToList();
        actions.Reverse();
        
        for (int i = 0; i < _actionIcons.Count/2; i++)
        {
            _actionIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i + normalizedTime) * _line.rect.width / _actionBar.TurnCount, 0);
            _actionIcons[i].SetTurnWidth(_line.rect.width / _actionBar.TurnCount).SetAction(actions[i]);
        }
        for (int i = _actionIcons.Count/2; i < _actionIcons.Count; i++)
        {
            _actionIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i + normalizedTime) * _line.rect.width / _actionBar.TurnCount, 0);
            _actionIcons[i].SetTurnWidth(_line.rect.width / _actionBar.TurnCount).SetAction(history[i - _actionIcons.Count/2]);
        }
    }
} 
