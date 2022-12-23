using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class handles the UI of rebinding keys
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>GameSettingManager</term>
///         <term>Prefab</term>
///         <term>GameSettingManager</term>
///         <term>This prefab contains the scripts that handle applying game settings</term>
///     </item>
///     <item>
///         <term>InputManager</term>
///         <term>Prefab</term>
///         <term>InputManager</term>
///         <term>This prefab contains the components that handle player input</term>
///     </item>
///     <item>
///         <term>SettingsMenu</term>
///         <term>Prefab</term>
///         <term>KeybindingUI.cs</term>
///         <term>This script handles the UI components to change keybindings</term>
///     </item>
/// </list>
public class KeybindingUI : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("The action that this UI element represents")]
    private InputActionReference _actionReference;
    
    [SerializeField] 
    [Tooltip("The index of the action binding in the list of keybindings")]
    private int _bindingIndex;

    [SerializeField] 
    [Tooltip("The name of the action to be displayed")]
    private string _actionName;

    [SerializeField] 
    [Tooltip("A reference to the action name UI text element")]
    private TMP_Text _actionNameUI;
    
    [SerializeField] 
    [Tooltip("A reference to the action button UI element")]
    private Button _actionButtonUI;
    
    [SerializeField] 
    [Tooltip("A reference to the action keybinding UI text element")]
    private TMP_Text _actionBindingUI;

    private KeybindingSettings _keybindingSettings;
    
    private void Start()
    {
        _keybindingSettings = FindObjectOfType<KeybindingSettings>();
        _keybindingSettings.SuccessfulRebinding += OnSuccessfulRebinding;
        
        _actionNameUI.text = _actionName;
        _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);

        _actionButtonUI.onClick.AddListener(OnActionRebind);
    }

    private void OnActionRebind()
    {
        _actionBindingUI.text = "...";
        
        _keybindingSettings.RemapKeyboardAction(_actionReference.action, _bindingIndex);
    }
    
    private void OnSuccessfulRebinding(InputAction action)
    {
        _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);
    }

    /// <summary>
    /// Refreshes the button binding UI to a new binding if changed
    /// </summary>
    public void RefreshRebindingButton()
    {
        _actionBindingUI.text = _actionReference.action.GetBindingDisplayString(_bindingIndex);
    }
}


