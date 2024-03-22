using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private KeyCode _interactKey = KeyCode.F;
    [SerializeField] private TextMeshProUGUI _promptGUI;
    
    private Interactable _currentInteractable;

    private void Update()
    {
        Interactable interactable = Physics.OverlapBox(transform.position + transform.forward, Vector3.one * 0.1f, Quaternion.identity, _interactableLayer).FirstOrDefault()?.GetComponent<Interactable>();

        if (interactable != _currentInteractable)
        {
            if (_currentInteractable != null)
            {
                _promptGUI.text = "";
                _currentInteractable.OnUnfocus();
            }

            _currentInteractable = interactable;

            if (_currentInteractable != null)
            {
                _promptGUI.text = _currentInteractable.PromptText.Replace("?", _interactKey.ToString());
                _currentInteractable.OnFocus();
            }
        }

        if (Input.GetKeyDown(_interactKey))
        {
            _currentInteractable?.OnInteract();
        }
    }
}
