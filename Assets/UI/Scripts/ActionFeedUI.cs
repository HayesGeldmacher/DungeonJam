using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFeedUI : MonoBehaviour
{
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private CombatBanner _combatBannerPrefab;

    private void OnEnable()
    {
        _combatManager.OnActionStart += HandleAction;
    }

    private void OnDisable()
    {
        _combatManager.OnActionStart -= HandleAction;
    }

    private void HandleAction(Action action)
    {
        if (action.GetDescription() == "") return;
        CombatBanner banner = Instantiate(_combatBannerPrefab, transform);
        banner.CallAppear(action.GetDescription());
        // banner.SetText(action.GetDescription());
    }
}
