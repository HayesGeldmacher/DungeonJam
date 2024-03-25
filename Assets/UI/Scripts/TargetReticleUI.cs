using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetReticleUI : MonoBehaviour
{
    [SerializeField] private Image _reticle;
    [SerializeField] private CombatAgent _agent;
    private bool _isTargeted = false;

    public void SetAgent(CombatAgent agent)
    {
        _agent = agent;
        _reticle.enabled = (agent != null && agent.TargetedReticlePosition != null) ? _isTargeted : false;
    }

    public void SetTargeted(bool isTargeted)
    {
        _isTargeted = isTargeted;
        if (_agent != null && _agent.TargetedReticlePosition != null)
        {
            _reticle.enabled = isTargeted;
        }
    }

    private void Update()
    {
        if (_agent != null && _agent.TargetedReticlePosition != null)
        {
            // Use the screen space canvas and the scale factor to convert the world position to screen position
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(_agent.TargetedReticlePosition.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)FindObjectOfType<Canvas>().transform, screenPosition, Camera.main, out Vector2 localPoint);
            transform.localPosition = new Vector3(localPoint.x, localPoint.y, transform.localPosition.z);
        }
    }
}
