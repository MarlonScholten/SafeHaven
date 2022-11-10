using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameObjectWidget : MonoBehaviour
{
    [SerializeField] 
    private GameObject debugKeyValueWidget;
    
    [SerializeField] 
    private GameObject debugHeaderWidget;
    
    private GameObject _debugGameObject;
    private IDebuggableObject[] _debugGameObjectScripts;
    private Dictionary<string, DebugKeyValueWidget> _debugWidgets = new();

    public void Initialize(GameObject debugGameObject)
    {
        _debugGameObject = debugGameObject;
        _debugGameObjectScripts = _debugGameObject.GetComponents<IDebuggableObject>();

        CreateDebugHeader(_debugGameObject.name);
        StartCoroutine(UpdateDebugWidgetCoroutine());
    }

    private IEnumerator UpdateDebugWidgetCoroutine()
    {
        while (true)
        {
            UpdateDebugWidget();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void UpdateDebugWidget()
    {
        foreach (IDebuggableObject debugGameObjectScript in _debugGameObjectScripts)
        {
            foreach (var debugValue in debugGameObjectScript.GetDebugValues())
            {
                SetDebugValue(debugValue.Key, debugValue.Value);
            }
        }
    }
    
    private void SetDebugValue(string variableName, string variableValue)
    {
        if (!_debugWidgets.ContainsKey(variableName))
            _debugWidgets.Add(variableName, Instantiate(debugKeyValueWidget, transform).GetComponent<DebugKeyValueWidget>());

        if (_debugWidgets.TryGetValue(variableName, out var debugWidget))
            debugWidget.SetVariableName(variableName).SetVariableValue(variableValue);
    }
    
    private void CreateDebugHeader(string headerName)
    {
        Instantiate(debugHeaderWidget, transform)
            .GetComponent<DebugHeaderWidget>()
            .SetHeaderText(headerName);
    }
}
