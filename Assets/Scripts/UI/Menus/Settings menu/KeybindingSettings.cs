using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class handles the logic of rebinding keys
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
public class KeybindingSettings : MonoBehaviour
{
    /// <summary>
    /// Event that is called when a rebinding has been applied or has been canceled by the user.
    /// </summary>
    public Action<InputAction> SuccessfulRebinding;
    
    [SerializeField]
    [Tooltip("The input actions asset that should be overriden")]
    private InputActionAsset _controls;
    
    private Dictionary<string, string> _overridesDictionary = new();

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/controlsOverrides.dat"))
        {
            LoadControlOverrides();
        }
    }
    
    /// <summary>
    /// Resets all user keybindings to default
    /// </summary>
    public void ResetKeybindings()
    {
        if (File.Exists(Application.persistentDataPath + "/controlsOverrides.dat"))
        {
            File.Delete(Application.persistentDataPath + "/controlsOverrides.dat");
        }
        
        _controls.RemoveAllBindingOverrides();
    }

    /// <summary>
    /// Method <c>RemapKeyboardAction</c> listens to the next keypress from the user and overrides the specified action keybinding
    /// This is then also saved to a file. This method is canceled by pressing escape.
    /// </summary>
    /// <param name="actionToRebind">The action to be rebound</param>
    /// <param name="targetBinding">The index of the keybinding in the action</param>
    public void RemapKeyboardAction(InputAction actionToRebind, int targetBinding)
    {
        var rebindOperation = actionToRebind.PerformInteractiveRebinding(targetBinding)
            .WithControlsHavingToMatchPath("<Keyboard>")
            .WithBindingGroup("Keyboard")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(operation => SuccessfulRebinding?.Invoke(null))
            .OnComplete(operation => {
                operation.Dispose();
                AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);
                SaveControlOverrides();
                SuccessfulRebinding?.Invoke(actionToRebind);
            })
            .Start();
    }

    /// <summary>
    /// Method <c>RemapGamepadAction</c> listens to the next gamepad press from the user and overrides the specified action keybinding
    /// This is then also saved to a file. This method is canceled by pressing escape.
    /// </summary>
    /// <param name="actionToRebind">The action to be rebound</param>
    /// <param name="targetBinding">The index of the keybinding in the action</param>
    public void RemapGamepadAction(InputAction actionToRebind, int targetBinding)
    {
        var rebindOperation = actionToRebind.PerformInteractiveRebinding(targetBinding)
            .WithControlsHavingToMatchPath("<Gamepad>")
            .WithBindingGroup("Gamepad")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(operation => SuccessfulRebinding?.Invoke(null))
            .OnComplete(operation => {
                operation.Dispose();
                AddOverrideToDictionary(actionToRebind.id, actionToRebind.bindings[targetBinding].effectivePath, targetBinding);
                SaveControlOverrides();
                SuccessfulRebinding?.Invoke(actionToRebind);
            })
            .Start();
    }

    private void AddOverrideToDictionary(Guid actionId, string path, int bindingIndex)
    {
        string key = string.Format("{0} : {1}", actionId.ToString(), bindingIndex);

        if (_overridesDictionary.ContainsKey(key))
        {
            _overridesDictionary[key] = path;
        }
        else
        {
            _overridesDictionary.Add(key, path);
        }
    }

    private void SaveControlOverrides()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/controlsOverrides.dat", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, _overridesDictionary);
        file.Close();
    }

    private void LoadControlOverrides()
    {
        
        if (!File.Exists(Application.persistentDataPath + "/controlsOverrides.dat"))
        {
            return;
        }

        FileStream file = new FileStream(Application.persistentDataPath + "/controlsOverrides.dat", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        _overridesDictionary = bf.Deserialize(file) as Dictionary<string, string>;
        file.Close();

        foreach (var item in _overridesDictionary)
        {
            string[] split = item.Key.Split(new string[] { " : " }, StringSplitOptions.None);
            Guid id = Guid.Parse(split[0]);
            int index = int.Parse(split[1]);
            _controls.FindAction(id)?.ApplyBindingOverride(index, item.Value);
        }
    }
}
