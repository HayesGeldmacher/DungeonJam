using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ActionPreviewUI : MonoBehaviour
{
    [SerializeField] private ActionIconUI _actionIconPrefab;
    [SerializeField] private RectTransform _line;
    [SerializeField] private CombatAgent _combatAgent;
    private ActionBarUI _actionBar;
    private List<ActionIconUI> _actionIcons = new List<ActionIconUI>();

    public void SetCombatAgent(CombatAgent agent)
    {
        _combatAgent = agent;
    }

    private void Awake()
    {
        _actionBar = GetComponentInParent<ActionBarUI>();
        for (int i = 0; i < _actionBar.TurnCount*2; i++)
        {
            _actionIcons.Add(Instantiate(_actionIconPrefab, _line));
        }
    }

    private void Update()
    {
        float normalizedTime = _actionBar.CombatManager.NormalizedTurnTime;

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
            _actionIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i + normalizedTime) * _actionBar.TurnWidth, 0);
            _actionIcons[i].SetAction(actions[i]);
        }
        for (int i = _actionIcons.Count/2; i < _actionIcons.Count; i++)
        {
            _actionIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i + normalizedTime) * _actionBar.TurnWidth, 0);
            _actionIcons[i].SetAction(history[i - _actionIcons.Count/2]);
        }
    }
} 
