using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIconUI : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private Image _preparationIcon = null;
    [SerializeField] private Image _recoveryIcon = null;

    private Action _action = null;

    public void SetAction(Action action)
    {
        /**
         * THIS IS UGLY FILTHY EVIL CODE AND I HATE IT
         * BUT IT WORKS SO I'M NOT GOING TO FIX IT
        **/
        _action = action;
        _icon.sprite = action?.Icon;
        _icon.gameObject.SetActive(_icon.sprite != null);
        _preparationIcon.gameObject.SetActive(action != null && action.PreparationTurns > 0);
        _recoveryIcon.gameObject.SetActive(action != null && !(action is NothingAction || action is PreparationAction || action is RecoveryAction));
        if (action != null && action.PreparationTurns > 0)
            _preparationIcon.rectTransform.offsetMax = new Vector2(action.PreparationTurns * GetComponentInParent<ActionBarUI>().TurnWidth - _icon.rectTransform.rect.width, _preparationIcon.rectTransform.offsetMax.y);
        if (action != null)
            _recoveryIcon.rectTransform.offsetMin = new Vector2((1 + action.RecoveryTurns) * -GetComponentInParent<ActionBarUI>().TurnWidth + _icon.rectTransform.rect.width, _recoveryIcon.rectTransform.offsetMin.y);
    }


}
