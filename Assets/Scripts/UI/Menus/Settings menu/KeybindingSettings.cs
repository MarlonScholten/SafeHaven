using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindingSettings : MonoBehaviour
{
    public InputActionAsset Controls;
    public Action<InputAction> SuccessfulRebinding;

    public Dictionary<string, string> OverridesDictionary = new Dictionary<string, string>();

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/controlsOverrides.dat"))
        {
            LoadControlOverrides();
        }
    }

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

        if (OverridesDictionary.ContainsKey(key))
        {
            OverridesDictionary[key] = path;
        }
        else
        {
            OverridesDictionary.Add(key, path);
        }
    }

    private void SaveControlOverrides()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/controlsOverrides.dat", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, OverridesDictionary);
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
        OverridesDictionary = bf.Deserialize(file) as Dictionary<string, string>;
        file.Close();

        foreach (var item in OverridesDictionary)
        {
            string[] split = item.Key.Split(new string[] { " : " }, StringSplitOptions.None);
            Guid id = Guid.Parse(split[0]);
            int index = int.Parse(split[1]);
            Controls.FindAction(id).ApplyBindingOverride(index, item.Value);
        }
    }
}
