using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleEvent : UnityEvent<bool>
{
}

public class DebugUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject debugContainer;

    [SerializeField] 
    private GameObject debugGameObjectWidget;

    [SerializeField]
    private List<GameObject> debugGameObjects;

    [SerializeField] 
    private List<DebugHighlight> highlightGameObjects;

    public ToggleEvent ToggleDebuggingTools;

    private bool _debuggerEnabled;
    private Canvas _canvasComponent;

    private void Awake()
    {
        ToggleDebuggingTools = new ToggleEvent();
    }

    void Start()
    {
        ToggleDebuggingTools.Invoke(_debuggerEnabled);
        
        _canvasComponent = GetComponent<Canvas>();
        _canvasComponent.enabled = _debuggerEnabled;
        
        ClearWidgets();

        foreach (GameObject debugGameObject in debugGameObjects)
        {
            Instantiate(debugGameObjectWidget, debugContainer.transform)
                .GetComponent<DebugGameObjectWidget>()
                .Initialize(debugGameObject);
        }
    }

    private void ClearWidgets()
    {
        for (int i = debugContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(debugContainer.transform.GetChild(i).gameObject);
        }
    }

    public void AddHighlightGameObject(DebugHighlight gameObjectToAdd)
    {
        highlightGameObjects.Add(gameObjectToAdd);
    }

    public void AddDebugGameObject(GameObject gameObjectToAdd)
    {
        debugGameObjects.Add(gameObjectToAdd);
        
        Instantiate(debugGameObjectWidget, debugContainer.transform)
            .GetComponent<DebugGameObjectWidget>()
            .Initialize(gameObjectToAdd);
    }

    private void OnShowDebuggingTools()
    {
        _debuggerEnabled = !_debuggerEnabled;
        
        ToggleDebuggingTools.Invoke(_debuggerEnabled);

        _canvasComponent.enabled = _debuggerEnabled;
        
        foreach (DebugHighlight highlightGameObject in highlightGameObjects)
        {
            if (_debuggerEnabled) highlightGameObject.Highlight();
            else highlightGameObject.UnHighlight();
        }
    }
}
