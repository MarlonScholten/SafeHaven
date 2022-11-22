using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeybindingUI : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private int _bindingIndex;

    [SerializeField] private string _actionName;

    [SerializeField] private TMP_Text _actionNameUI;
    [SerializeField] private Button _actionButtonUI;
    [SerializeField] private TMP_Text _actionBindingUI;

    private KeybindingSettings _keybindingSettings;
    
    private void Start()
    {
        _keybindingSettings = FindObjectOfType<KeybindingSettings>();
        
        _actionNameUI.text = _actionName;
        _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);

        _actionButtonUI.onClick.AddListener(OnActionRebind);
    }

    private void OnActionRebind()
    {
        _actionBindingUI.text = "...";
        
        _keybindingSettings.SuccessfulRebinding += OnSuccessfulRebinding;
        _keybindingSettings.RemapKeyboardAction(_actionReference.action, _bindingIndex);
    }
    
    private void OnSuccessfulRebinding(InputAction action)
    {
        _keybindingSettings.SuccessfulRebinding -= OnSuccessfulRebinding;
        
        _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);
    }
}


