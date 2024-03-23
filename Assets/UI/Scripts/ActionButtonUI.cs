using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Action _action;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;

    private void Awake()
    {
        SetAction(_action);
    }

    public ActionButtonUI SetAction(Action action)
    {
        _action = action;
        _icon.sprite = action?.Icon;
        _nameText.text = action?.Name;
        return this;
    }
}
