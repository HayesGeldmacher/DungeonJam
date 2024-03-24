using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private CombatAgent _agent;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void SetCombatAgent(CombatAgent agent)
    {
        _agent = agent;
    }

    private void Update()
    {
        _slider.value = (float)_agent.CurrentHealth / _agent.MaxHealth;
        _healthText.text = $"{_agent.CurrentHealth} / {_agent.MaxHealth}";
    }
}
