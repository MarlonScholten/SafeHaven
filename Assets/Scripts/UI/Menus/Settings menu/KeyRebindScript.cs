using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebindScript : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private int _bindingIndex;

    [SerializeField] private string _actionName;

    [SerializeField] private TMP_Text _actionNameUI;
    [SerializeField] private Button _actionButtonUI;
    [SerializeField] private TMP_Text _actionBindingUI;

    private void Start()
    {
        _actionNameUI.text = _actionName;
        _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);
        
        _actionButtonUI.onClick.AddListener(OnActionRebind);
    }

    private void OnActionRebind()
    {
        _actionBindingUI.text = "...";
        
        RemapKeyboardAction(_actionReference.action, _bindingIndex);
    }

    private void RemapKeyboardAction(InputAction actionToRebind, int targetBinding)
    {
        actionToRebind.PerformInteractiveRebinding(targetBinding)
            .WithControlsHavingToMatchPath("<Keyboard>")
            .WithBindingGroup("Keyboard")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(_ =>
            {
                _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);
            })
            .OnComplete(operation => {
                operation.Dispose();
                
                _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);
            })
            .Start();
    }
}


