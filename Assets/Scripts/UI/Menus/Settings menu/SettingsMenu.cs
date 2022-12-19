using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class is a container that holds different items that change user settings.
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
public class SettingsMenu : MonoBehaviour
{
    private GameObject _previousMenu;

    [SerializeField]
    [Tooltip("A reference to the button UI element that exits this menu")]
    private Button _exitButton;
    
    [SerializeField]
    [Tooltip("A reference to the button UI element that resets the keybindings")]
    private Button _resetKeybindingsButton;
    
    [SerializeField]
    [Tooltip("A reference to the input actions asset")]
    private InputActionAsset _controls;

    private bool _confirmation = false;

    private void Start()
    {
        _exitButton.onClick.AddListener(CloseMenu);
        _resetKeybindingsButton.onClick.AddListener(ResetKeybindings);
        
        _exitButton.Select();
    }

    /// <summary>
    /// Sets the menu that spawned this menu, closing this menu reactivates this previous menu
    /// </summary>
    /// <param name="previousMenu">The menu that is reactivated when this menu is closed</param>
    public void SetPreviousMenu(GameObject previousMenu)
    {
        _previousMenu = previousMenu;
    }

    private void CloseMenu()
    {
        // User accepts the keybindings unbound warning
        if (_confirmation)
        {
            _previousMenu.SetActive(true);
            Destroy(gameObject);
            return;
        }

        // Check if keybindings are unbound
        foreach (var inputBinding in _controls.FindActionMap("Player").bindings)
        {
            if (inputBinding.effectivePath == "")
            {
                TMP_Text exitText = _exitButton.GetComponentInChildren<TMP_Text>();
                
                exitText.SetText("You have unbound settings,\nare you sure you want to continue?");
                exitText.color = Color.red;
                exitText.fontSize = 16.0f;

                _confirmation = true;
                
                StartCoroutine(ConfirmationTimeout());
                return;
            }
        }
        
        // All keybindings are bound
        _previousMenu.SetActive(true);
        Destroy(gameObject);
    }

    private IEnumerator ConfirmationTimeout()
    {
        yield return new WaitForSeconds(3.0f);
        
        TMP_Text exitText = _exitButton.GetComponentInChildren<TMP_Text>();
                
        exitText.SetText("Back");
        exitText.color = Color.white;
        exitText.fontSize = 24.0f;

        _confirmation = false;
    }

    private void ResetKeybindings()
    {
        FindObjectOfType<KeybindingSettings>()?.ResetKeybindings();

        foreach (KeybindingUI keybindingUI in GetComponentsInChildren<KeybindingUI>())
        {
            keybindingUI.RefreshRebindingButton();
        }
    }
}
